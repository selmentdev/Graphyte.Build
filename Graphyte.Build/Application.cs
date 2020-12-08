using Graphyte.Build.Evaluation;
using Graphyte.Build.Generators;
using Graphyte.Build.Resolving;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Graphyte.Build
{
    public class Application
    {
        private static readonly Version Current = new Version(1, 0, 0, 0);

        public sealed class Options
        {
            public FileInfo Profile;
            public string Platform;
            public string Toolchain;
            public string Generator;
            public string OutputPath;

            [Conditional("TRACE")]
            [Conditional("DEBUG")]
            public void Dump()
            {
                Trace.WriteLine($@"Options:");
                Trace.Indent();

                Trace.WriteLine($@"Profile file: {this.Profile}");
                Trace.WriteLine($@"Platform:     {this.Platform}");
                Trace.WriteLine($@"Toolchain:    {this.Toolchain}");
                Trace.WriteLine($@"Generator:    {this.Generator}");
                Trace.WriteLine($@"OutputPath:   {this.OutputPath}");

                Trace.Unindent();
            }
        }

        private readonly Options m_Options;

        private readonly Profile m_Profile;

        private readonly PlatformsProvider m_PlatformsProvider
            = new PlatformsProvider();

        private readonly GeneratorsProvider m_GeneratorsProvider
            = new GeneratorsProvider();

        private readonly SolutionsProvider m_SolutionsProvider
            = new SolutionsProvider();

        private readonly PlatformType m_PlatformType;
        private readonly ToolchainType m_ToolchainType;
        private readonly GeneratorType m_GeneratorType;
        private readonly ConfigurationType[] m_ConfigurationTypes;
        private readonly BasePlatformFactory[] m_PlatformFactories;
        private readonly ArchitectureType[] m_Architectures;
        private readonly BaseGenerator m_Generator;
        private readonly Solution[] m_Solutions;

        private readonly struct ConfigurationMapping
        {
            public readonly ConfigurationType Type;
            public readonly ConfigurationFlavour Flavour;

            public ConfigurationMapping(
                ConfigurationType type,
                ConfigurationFlavour flavour)
            {
                this.Type = type;
                this.Flavour = flavour;
            }
        }

        private static readonly ConfigurationMapping[] m_Configurations = new[]
        {
            new ConfigurationMapping(ConfigurationType.Debug, ConfigurationFlavour.None),
            new ConfigurationMapping(ConfigurationType.Debug, ConfigurationFlavour.Client),
            new ConfigurationMapping(ConfigurationType.Debug, ConfigurationFlavour.Editor),
            new ConfigurationMapping(ConfigurationType.Debug, ConfigurationFlavour.Server),
            new ConfigurationMapping(ConfigurationType.DebugGame, ConfigurationFlavour.None),
            new ConfigurationMapping(ConfigurationType.DebugGame, ConfigurationFlavour.Client),
            new ConfigurationMapping(ConfigurationType.DebugGame, ConfigurationFlavour.Editor),
            new ConfigurationMapping(ConfigurationType.DebugGame, ConfigurationFlavour.Server),
            new ConfigurationMapping(ConfigurationType.Development, ConfigurationFlavour.None),
            new ConfigurationMapping(ConfigurationType.Development, ConfigurationFlavour.Client),
            new ConfigurationMapping(ConfigurationType.Development, ConfigurationFlavour.Editor),
            new ConfigurationMapping(ConfigurationType.Development, ConfigurationFlavour.Server),
            new ConfigurationMapping(ConfigurationType.Release, ConfigurationFlavour.None),
            new ConfigurationMapping(ConfigurationType.Release, ConfigurationFlavour.Client),
            new ConfigurationMapping(ConfigurationType.Release, ConfigurationFlavour.Server),
            new ConfigurationMapping(ConfigurationType.Testing, ConfigurationFlavour.None),
            new ConfigurationMapping(ConfigurationType.Testing, ConfigurationFlavour.Client),
            new ConfigurationMapping(ConfigurationType.Testing, ConfigurationFlavour.Server),
        };

        [Conditional("TRACE")]
        [Conditional("DEBUG")]
        private void DumpProviders()
        {
            Trace.WriteLine("Platforms:");
            Trace.Indent();

            foreach (var platform in this.m_PlatformsProvider.Platforms)
            {
                Trace.WriteLine($@"{platform.PlatformType}-{platform.ArchitectureType}-{platform.ToolchainType}");
            }

            Trace.Unindent();

            Trace.WriteLine("Generators:");
            Trace.Indent();

            foreach (var generator in this.m_GeneratorsProvider.Generators)
            {
                Trace.WriteLine(generator);
            }

            Trace.Unindent();

            Trace.WriteLine("Solutions:");
            Trace.Indent();

            foreach (var solution in this.m_SolutionsProvider.Solutions)
            {
                Trace.WriteLine(solution);
            }

            Trace.Unindent();

            Trace.WriteLine("Architectures:");
            Trace.Indent();

            foreach (var architecture in this.m_Architectures)
            {
                Trace.WriteLine(architecture);
            }

            Trace.Unindent();

            Trace.WriteLine("Configurations:");
            Trace.Indent();

            foreach (var configuration in m_Configurations)
            {
                Trace.WriteLine($@"{configuration.Type} {configuration.Flavour}");
            }

            Trace.Unindent();
        }

        private static void EmitLogo()
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Trace.WriteLine(@"   ______                 __          __         ____        _ __    __");
            Trace.WriteLine(@"  / ____/________ _____  / /_  __  __/ /____    / __ )__  __(_) /___/ /");
            Trace.WriteLine(@" / / __/ ___/ __ `/ __ \/ __ \/ / / / __/ _ \  / __  / / / / / / __  /");
            Trace.WriteLine(@"/ /_/ / /  / /_/ / /_/ / / / / /_/ / /_/  __/ / /_/ / /_/ / / / /_/ /");
            Trace.WriteLine(@"\____/_/   \__,_/ .___/_/ /_/\__, /\__/\___(_)_____/\__,_/_/_/\__,_/");
            Trace.WriteLine(@"               /_/          /____/");
            Trace.WriteLine(string.Empty);

            Console.ForegroundColor = previous;
        }

        private Application(string[] args)
        {
            //
            // Setup GC
            //

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;


            //
            // Setup console tracing.
            //

            Trace.Listeners.Add(new ConsoleTraceListener());
            Application.EmitLogo();

            Trace.WriteLine($@"Graphyte Build version {Application.Current}");
            Trace.WriteLine($@"{RuntimeInformation.OSDescription} ({RuntimeInformation.FrameworkDescription})");
            Trace.WriteLine($@"Working directory: {Environment.CurrentDirectory}");

            //
            // Parse options.
            //

            this.m_Options = CommandLineParser.Parse<Options>(args);
            this.m_Options.Dump();


            //
            // Map to enums.
            //

            this.m_PlatformType = PlatformType.Create(this.m_Options.Platform);
            this.m_ToolchainType = ToolchainType.Create(this.m_Options.Toolchain);
            this.m_GeneratorType = GeneratorType.Create(this.m_Options.Generator);

            this.m_ConfigurationTypes = Enum.GetValues(typeof(ConfigurationType))
                .Cast<ConfigurationType>()
                .ToArray();


            //
            // Parse profile.
            //

            var bytes = File.ReadAllBytes(this.m_Options.Profile.FullName);
            this.m_Profile = Profile.Parse(bytes);


            //
            // Resolve specific platform and toolchain.
            //

            this.m_Generator = this.m_GeneratorsProvider.Generators
                .First(x => x.GeneratorType == this.m_GeneratorType)
                .Create(this.m_Profile);


            //
            // Factories have unique architecture types.
            //

            this.m_PlatformFactories = this.m_PlatformsProvider.Platforms.Where(
                x => x.PlatformType == this.m_PlatformType && x.ToolchainType == this.m_ToolchainType)
                .ToArray();

            this.m_Architectures = this.m_PlatformFactories.Select(x => x.ArchitectureType).ToArray();

            Trace.Assert(this.m_Architectures.Distinct().Count() == this.m_Architectures.Length);

            this.m_Solutions = this.m_SolutionsProvider.Create();
        }

        private readonly struct FactoryContext
        {
            public readonly BasePlatform Platform;
            public readonly BaseToolchain Toolchain;
            public readonly BasePlatformFactory Factory;

            public FactoryContext(
                BasePlatformFactory factory,
                Profile profile)
            {
                this.Factory = factory;
                this.Platform = factory.CreatePlatform(profile);
                this.Toolchain = factory.CreateToolchain(profile);
            }
        }

        private int Run()
        {
            Debug.Assert(this.m_ConfigurationTypes != null);
            Debug.Assert(this.m_PlatformFactories != null);
            Debug.Assert(this.m_Architectures != null);
            Debug.Assert(this.m_Solutions != null);

            this.DumpProviders();

            var platformToolchains = this.m_PlatformFactories
                .Select(x => new FactoryContext(x, this.m_Profile));

            foreach (var solution in this.m_Solutions)
            {
                var evaluated = platformToolchains.Select(context =>
                {
                    var resolvedSolutions = m_Configurations.Select(configuration =>
                    {
                        var targetTuple = new TargetTuple(
                            context.Factory.PlatformType,
                            context.Factory.ArchitectureType,
                            context.Factory.ToolchainType,
                            configuration.Type,
                            configuration.Flavour);

                        var result = new ResolvedSolution(solution, targetTuple);
                        result.Configure();
                        result.Resolve();
                        return result;
                    }).ToArray();

                    return new EvaluatedSolution(
                        context.Platform,
                        context.Toolchain,
                        solution,
                        this.m_Profile,
                        resolvedSolutions);
                }).ToArray();

                this.m_Generator.Generate(
                    this.m_Options.OutputPath,
                    this.m_PlatformType,
                    this.m_ToolchainType,
                    solution,
                    evaluated);
            }

            return 0;
        }

        public static int Main(string[] args)
        {
            //
            // Start timing.
            //

            var watch = Stopwatch.StartNew();

            try
            {
                var app = new Application(args);
                return app.Run();
            }
            catch (Exception e)
            {
                ReportException(e);
                return -1;
            }
            finally
            {
                watch.Stop();
                Trace.WriteLine($@"Excution time: {watch.Elapsed.TotalSeconds:0.0.00}");
            }
        }

        private static void ReportException(Exception e)
        {
            while (e != null)
            {
                Trace.WriteLine($@"Failed with exception: {e.Message}");

#if TRACE
                Trace.WriteLine("Stacktrace:");
                Trace.WriteLine(e.StackTrace);
#endif

                e = e.InnerException;
            }
        }
    }
}
