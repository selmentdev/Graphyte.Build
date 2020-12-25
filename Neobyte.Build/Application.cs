using Neobyte.Build.Core;
using Neobyte.Build.Evaluation;
using Neobyte.Build.Framework;
using Neobyte.Build.Platforms;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;

[assembly: Neobyte.Build.Core.TypesProvider]

namespace Neobyte.Build
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

            Trace.WriteLine($@"Neobyte Build version {Application.Current}");
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

namespace Neobyte.Build
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
            public FileInfo LogFile;

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
                Trace.WriteLine($@"Log file:  {this.LogFile}");

                Trace.Unindent();
            }
        }

        private static void PrintLogo()
        {
            // Slant: (http://patorjk.com/software/taag/#p=display&f=Slant&t=Anemone.Build
            var previous = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    ___                                            ____        _ __    __");
            Console.WriteLine("   /   |  ____  ___  ____ ___  ____  ____  ___    / __ )__  __(_) /___/ /");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  / /| | / __ \\/ _ \\/ __ `__ \\/ __ \\/ __ \\/ _ \\  / __  / / / / / / __  / ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" / ___ |/ / / /  __/ / / / / / /_/ / / / /  __/ / /_/ / /_/ / / / /_/ /  ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("/_/  |_/_/ /_/\\___/_/ /_/ /_/\\____/_/ /_/\\___(_)_____/\\__,_/_/_/\\__,_/   ");
            Console.WriteLine();

            Console.ForegroundColor = previous;
        }

        private readonly Options m_Options;
        private readonly Profile m_Profile;

        private Application(string[] args)
        {
            this.m_Options = Core.CommandLineParser.Parse<Options>(args);
            this.m_Options.Dump();

            if (this.m_Options.LogFile != null)
            {
                Trace.Listeners.Add(new TextWriterTraceListener(this.m_Options.LogFile.FullName, "LogFile"));
            }

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

            var workspace = new Workspace(
                targetsProvider.Targets,
                modulesProvider.Modules,
                selectedPlatformsFactories,
                this.m_Profile);

            var evaluatedWorkspace = new EvaluatedWorkspace(workspace);

            evaluatedWorkspace.Dump();

            return evaluatedWorkspace.Targets.Count() > 0? 0 : 1;
        }

        private static void ValidatePlatformFactories(PlatformFactory[] platformFactories)
        {
            var duplicate = platformFactories
                .GroupBy(x => x.Architecture)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key);

            if (duplicate.Any())
            {
                throw new Exception($@"Duplicated architecture: {duplicate.First()}");
            }
        }
    }
}
