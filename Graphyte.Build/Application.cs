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
        private readonly PlatformsProvider m_PlatformsProvider = new PlatformsProvider();
        private readonly ToolchainsProvider m_ToolchainsProvider = new ToolchainsProvider();
        private readonly GeneratorsProvider m_GeneratorsProvider = new GeneratorsProvider();
        private readonly SolutionsProvider m_SolutionsProvider = new SolutionsProvider();

        [Conditional("TRACE")]
        [Conditional("DEBUG")]
        private void DumpProviders()
        {
            Trace.WriteLine("Platforms:");
            Trace.Indent();

            foreach (var platform in this.m_PlatformsProvider.Platforms)
            {
                Trace.WriteLine(platform);
            }

            Trace.Unindent();

            Trace.WriteLine("Toolchains:");
            Trace.Indent();

            foreach (var toolchain in this.m_ToolchainsProvider.Toolchains)
            {
                Trace.WriteLine(toolchain);
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

        private static void Validate(BasePlatform platform, BaseToolchain toolchain, BaseGenerator generator)
        {
            if (!platform.IsHostSupported)
            {
                throw new Exception($@"{platform} is not supported on host machine ({RuntimeInformation.OSDescription})");
            }

            if (!toolchain.IsHostSupported)
            {
                throw new Exception($@"{toolchain} is not supported on host machine ({RuntimeInformation.OSDescription})");
            }

            if (!generator.IsHostSupported)
            {
                throw new Exception($@"{generator} is not supported on host machine ({RuntimeInformation.OSDescription})");
            }
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

            var currentPlatform = this.m_PlatformsProvider.Create(typePlatform);
            var currentToolchain = this.m_ToolchainsProvider.Create(typeToolchain);
            var currentGenerator = this.m_GeneratorsProvider.Create(typeGenerator);

            Application.Validate(currentPlatform, currentToolchain, currentGenerator);

            var currentArchitectures = currentPlatform.Architectures;

            currentPlatform.Initialize(this.m_Profile);
            currentToolchain.Initialize(this.m_Profile);
            currentGenerator.Initialize(this.m_Profile);


            Trace.WriteLine($@"Resolved platform:  {currentPlatform} ({currentPlatform.IsHostSupported})");
            Trace.WriteLine($@"Resolved toolchain: {currentToolchain} ({currentToolchain.IsHostSupported})");
            Trace.WriteLine($@"Resolved generator: {currentGenerator} ({currentGenerator.IsHostSupported})");

            //Trace.WriteLine($@"Toolchain paths: {currentToolchain}");
            //Trace.Indent();
            //foreach (var path in currentToolchain.IncludePaths)
            //{
            //    Trace.WriteLine(@$"inc: ""{path}""");
            //}
            //foreach (var path in currentToolchain.LibraryPaths)
            //{
            //    Trace.WriteLine(@$"inc: ""{path}""");
            //}
            //Trace.Unindent();

            Trace.WriteLine($@"Platform paths: {currentPlatform}");
            Trace.Indent();
            foreach (var path in currentPlatform.GetIncludePaths(currentArchitectures.First()))
            {
                Trace.WriteLine(@$"inc: ""{path}""");
            }
            foreach (var path in currentPlatform.GetLibraryPaths(currentArchitectures.First()))
            {
                Trace.WriteLine(@$"lib: ""{path}""");
            }
            Trace.Unindent();

            Trace.WriteLine($@"Supported architectures");
            Trace.Indent();

            foreach (var architecture in currentArchitectures)
            {
                Trace.WriteLine(architecture);
            }
            Trace.Unindent();


            var currentConfigurations = Enum.GetValues(typeof(ConfigurationType)).Cast<ConfigurationType>();
            Trace.WriteLine("Supported configurations");
            Trace.Indent();
            foreach (var configuration in currentConfigurations)
            {
                Trace.WriteLine(configuration);
            }
            Trace.Unindent();

            Trace.WriteLine("Targets");


            ////////////////////////////////////////////////////////////////
            var solutions = this.m_SolutionsProvider.Create();

            foreach (var solution in solutions)
            {
                foreach (var architecture in currentArchitectures)
                {
                    foreach (var configuration in currentConfigurations)
                    {
                        var tuple = new TargetTuple(typePlatform, architecture, typeToolchain, configuration);
                        var resolved = new ResolvedSolution(solution, tuple);

                        resolved.Configure(
                            currentToolchain,
                            currentGenerator,
                            currentPlatform);

                        resolved.Resolve();

                        Trace.WriteLine($@"{currentPlatform.Type}-{currentToolchain.Type}-{architecture}-{configuration}/");
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
