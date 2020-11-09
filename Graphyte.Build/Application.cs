using Graphyte.Build.Generators;
using Graphyte.Build.Platforms;
using Graphyte.Build.Resolving;
using Graphyte.Build.Toolchains;
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
            Trace.WriteLine($@"Graphyte Build version {Application.Current}");
            Trace.WriteLine($@"{RuntimeInformation.OSDescription} ({RuntimeInformation.FrameworkDescription})");

            this.DumpProviders();

            //
            // Parse options.
            //

            this.m_Options = CommandLineParser.Parse<Options>(args);
            this.m_Options.Dump();

            var typePlatform = PlatformType.Create(this.m_Options.Platform);
            var typeToolchain = ToolchainType.Create(this.m_Options.Toolchain);
            var typeGenerator = GeneratorType.Create(this.m_Options.Generator);

            //
            // Parse profile.
            //

            var bytes = File.ReadAllBytes(this.m_Options.Profile.FullName);
            this.m_Profile = Profile.Parse(bytes);

            //
            // Resolve specific platform and toolchain.
            //

            var currentGenerator = this.m_GeneratorsProvider.Create(typeGenerator);

            var currentConfigurations = Enum.GetValues(typeof(ConfigurationType)).Cast<ConfigurationType>();

            var platformFactories = this.m_PlatformsProvider.Platforms.Where(x =>
                x.PlatformType == typePlatform &&
                x.ToolchainType == typeToolchain);

            var solutions = this.m_SolutionsProvider.Create();

            foreach (var factory in platformFactories)
            {
                Trace.WriteLine($@"Factory: {factory}");

                var platform = factory.CreatePlatform(this.m_Profile);

                var toolchain = factory.CreateToolchain(this.m_Profile);

                foreach (var solution in solutions)
                {
                    foreach (var configuration in currentConfigurations)
                    {
                        var tuple = new TargetTuple(
                            factory.PlatformType,
                            factory.ArchitectureType,
                            factory.ToolchainType,
                            configuration,
                            ConfigurationFlavour.None);

                        var resolved = new ResolvedSolution(solution, tuple);

                        resolved.Configure(
                            toolchain,
                            currentGenerator,
                            platform);

                        resolved.Resolve();

                        Trace.WriteLine($@"{factory.PlatformType}-{factory.ToolchainType}-{factory.ArchitectureType}-{configuration}/");
                        Trace.Indent();

                        foreach (var target in resolved.Targets)
                        {
                            Trace.WriteLine(target.Name);
                            Trace.Indent();
                            foreach (var dependency in target.PrivateDependencies)
                            {
                                Trace.WriteLine($@"- {dependency.Name}");
                            }
                            Trace.Unindent();
                        }
                        Trace.Unindent();
                    }
                }
            }
        }

        private int Run()
        {
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
                Trace.WriteLine($@"Failed with exception: {e.Message}");

#if TRACE
                Trace.WriteLine("Stacktrace:");
                Trace.WriteLine(e.StackTrace);
#endif
                return -1;
            }
            finally
            {
                watch.Stop();
                Trace.WriteLine($@"Excution time: {watch.Elapsed.TotalSeconds:0.0.00}");
            }
        }
    }
}
