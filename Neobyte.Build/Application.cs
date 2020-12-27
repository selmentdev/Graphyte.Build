using Neobyte.Build.Core;
using Neobyte.Build.Evaluation;
using Neobyte.Build.Framework;
using Neobyte.Build.Generators;
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

        private static bool g_PrintLogo = true;

        [CommandLineOption("-NoLogo")]
        public static void OnNoLogo()
        {
            g_PrintLogo = false;
        }

        public static int Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            //
            // Setup GC
            //

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;


            var commandLine = new CommandLine(args);
            commandLine.Apply<Application>();

            //
            // Setup console tracing.
            //

            Trace.Listeners.Add(new ConsoleTraceListener());

            if (g_PrintLogo)
            {
                Application.PrintLogo();
            }

            Trace.WriteLine($@"Neobyte Build version {Application.Current}");
            Trace.WriteLine($@"{RuntimeInformation.OSDescription} ({RuntimeInformation.FrameworkDescription})");
            Trace.WriteLine($@"Working directory: {Environment.CurrentDirectory}");

            try
            {
                var app = new Application();
                commandLine.Apply(app);

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

        private static void ReportException(Exception? e)
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

        private Profile? m_Profile;

        private TargetPlatform? m_TargetPlatform;
        private TargetToolchain? m_TargetToolchain;
        private DirectoryInfo? m_OutputDirectory;

        [CommandLineOption("-Profile")]
        public void OnProfile(FileInfo path)
        {
            Trace.WriteLine($@"Profile: {path}");
            this.m_Profile = new Profile(path.OpenRead().ReadAllBytes());
        }

        [CommandLineOption("-LogFile")]
        public void OnLogFile(FileInfo path)
        {
            Trace.WriteLine($@"LogFile: {path}");
            Trace.Listeners.Add(new TextWriterTraceListener(path.FullName, "LogFile"));
        }

        [CommandLineOption("-Platform")]
        public void OnPlatform(TargetPlatform value)
        {
            Trace.WriteLine($@"Platform: {value}");
            this.m_TargetPlatform = value;
        }

        [CommandLineOption("-Toolchain")]
        public void OnToolchain(TargetToolchain? value)
        {
            Trace.WriteLine($@"Toolchain: {value}");
            this.m_TargetToolchain = value;
        }

        [CommandLineOption("-OutputPath")]
        public void OnOutputPath(DirectoryInfo? path)
        {
            Trace.WriteLine($@"OutputPath: {path}");
            this.m_OutputDirectory = path;
        }

        [CommandLineOption("-Generator")]
        public void OnGenerator(GeneratorType value)
        {
            Trace.WriteLine($@"Generator: {value}");
        }

        private Application()
        {
        }

        private int Run()
        {
            if (this.m_Profile == null)
            {
                throw new Exception("Profile file not specified");
            }

            if (this.m_OutputDirectory == null)
            {
                //throw new Exception("Invalid output directory");
            }

            if (this.m_TargetPlatform == null)
            {
                throw new Exception("Target platform not specified");
            }

            if (this.m_TargetToolchain == null)
            {
                throw new Exception("Target platform not specified");
            }

            var targetPlatform = this.m_TargetPlatform.Value;
            var targetToolchain = this.m_TargetToolchain.Value;

            var platformsProvider = new PlatformProvider();
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

            //evaluatedWorkspace.Dump();

            return evaluatedWorkspace.Targets.Count() > 0 ? 0 : 1;
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
