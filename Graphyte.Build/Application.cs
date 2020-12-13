using Graphyte.Build.Framework;
using Graphyte.Build.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using Graphyte.Build.Platforms;

[assembly: Graphyte.Build.Core.TypesProvider]

namespace Graphyte.Build
{
    public sealed partial class Application
    {
        private static readonly Version Current = new Version(1, 0, 0, 0);
        
        public static int Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            //
            // Setup GC
            //

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;


            //
            // Setup console tracing.
            //

            Trace.Listeners.Add(new ConsoleTraceListener());
            Application.PrintLogo();

            Trace.WriteLine($@"Graphyte Build version {Application.Current}");
            Trace.WriteLine($@"{RuntimeInformation.OSDescription} ({RuntimeInformation.FrameworkDescription})");
            Trace.WriteLine($@"Working directory: {Environment.CurrentDirectory}");

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
                Trace.WriteLine($@"Execution time: {watch.Elapsed.TotalSeconds:0.0.00}");
            }
        }

        private static void ReportException(Exception e)
        {
            while (e != null)
            {
                Trace.WriteLine($@"Failed with exception: {e.Message}");

#if TRACE
                Trace.WriteLine($@"Stacktrace:");
                Trace.WriteLine(e.StackTrace);
#endif

                e = e.InnerException;
            }
        }
    }
}

namespace Graphyte.Build
{
    public sealed partial class Application
    {
        public sealed class Options
        {
            public FileInfo Profile;
            public DirectoryInfo OutputPath;
            public string Platform;
            public string Toolchain;
            public string Generator;

            [Conditional("DEBUG")]
            public void Dump()
            {
                Trace.WriteLine("Options:");
                Trace.Indent();

                Trace.WriteLine($@"Profile:   {this.Profile}");
                Trace.WriteLine($@"Output:    {this.OutputPath}");
                Trace.WriteLine($@"Platform:  {this.Platform}");
                Trace.WriteLine($@"Toolchain: {this.Toolchain}");
                Trace.WriteLine($@"Generator: {this.Generator}");

                Trace.Unindent();
            }
        }

        private static void PrintLogo()
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

        private readonly Options m_Options;
        private readonly Profile m_Profile;

        private Application(string[] args)
        {
            this.m_Options = Core.CommandLineParser.Parse<Options>(args);
            this.m_Options.Dump();

            this.m_Profile = new Profile(this.m_Options.Profile.OpenRead().ReadAllBytes());
        }

        private int Run()
        {
            var targetPlatform = TargetPlatform.Create(this.m_Options.Platform);
            var targetToolchain = TargetToolchain.Create(this.m_Options.Toolchain);

            var platformsProvider = new Platforms.PlatformProvider();
            platformsProvider.Dump();

            var targetsProvider = new TargetRulesProvider();
            targetsProvider.Dump();

            var modulesProvider = new ModuleRulesProvider();
            modulesProvider.Dump();

            var selectedPlatformsFactories = platformsProvider.GetPlatformFactories(targetPlatform, targetToolchain);
            ValidatePlatformFactories(selectedPlatformsFactories);

            var selectedConfigurations = Enum
                .GetValues(typeof(TargetConfiguration))
                .Cast<TargetConfiguration>()
                .ToArray();

            foreach (var currentPlatformFactory in selectedPlatformsFactories)
            {
                foreach (var currentConfiguration in selectedConfigurations)
                {
                    var targetDescriptor = new TargetDescriptor(
                        currentPlatformFactory.TargetPlatform,
                        currentPlatformFactory.TargetArchitecture,
                        currentPlatformFactory.TargetToolchain,
                        currentConfiguration);

                    var currentPlatform = currentPlatformFactory.CreatePlatform(this.m_Profile);
                    var currentToolchain = currentPlatformFactory.CreateToolchain(this.m_Profile);

                    var targetContext = new TargetContext(currentPlatform, currentToolchain);

                    foreach (var currentTarget in targetsProvider.Targets)
                    {
                        var evaluatedTarget = new Evaluation.EvaluatedTargetRules(
                            currentTarget,
                            targetDescriptor,
                            targetContext,
                            modulesProvider.Modules);

                        foreach (var module in evaluatedTarget.ModuleRules)
                        {
                            Debug.WriteLine($@"{module.EvaluatedTargetRules.TargetDescriptor}-{currentTarget.Name}-{module.ModuleRules}");
                        }
                    }
                }
            }

            return 0;
        }

        private static void ValidatePlatformFactories(PlatformFactory[] platformFactories)
        {
            var duplicate = platformFactories
                .GroupBy(x => x.TargetArchitecture)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key);

            if (duplicate.Any())
            {
                throw new Exception($@"Duplicated architecture: {duplicate.First()}");
            }
        }
    }
}
