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

        private static readonly Cli.Option<bool> OptionHelp = new("--help", "Print this help.");

        private static readonly Cli.Option<bool> OptionNoBanner = new("--no-banner", "Don't print banner.");

        private static readonly Cli.Option<FileInfo> OptionProfileFile = new("--profile", "Profile file path.")
        {
            Argument = new Cli.Argument<FileInfo>("PROFILE")
            {
                Arity = Cli.ArgumentArity.One
            }
        };
        private static readonly Cli.Option<FileInfo> OptionLogFile = new("--log", "Log file path.")
        {
            Argument = new Cli.Argument<FileInfo>("LOG")
            {
                Arity = Cli.ArgumentArity.One
            }
        };
        private static readonly Cli.Option<DirectoryInfo> OptionOutputPath = new("--output", "Output generator working directory.")
        {
            Argument = new Cli.Argument<DirectoryInfo>("OUTPUT")
            {
                Arity = Cli.ArgumentArity.One
            }
        };
        private static readonly Cli.Option<TargetPlatform> OptionTargetPlatform = new("--platform", "Target platform.")
        {
            Argument = new Cli.Argument<TargetPlatform>("PLATFORM")
            {
                Arity = Cli.ArgumentArity.One
            }
        };
        private static readonly Cli.Option<TargetToolchain> OptionTargetToolchain = new("--toolchain", "Target toolchain.")
        {
            Argument = new Cli.Argument<TargetToolchain>("TOOLCHAIN")
            {
                Arity = Cli.ArgumentArity.One
            }
        };
        private static readonly Cli.Option<GeneratorType> OptionGeneratorType = new("--generator", "Target generator type.")
        {
            Argument = new Cli.Argument<GeneratorType>("GENERATOR")
            {
                Arity = Cli.ArgumentArity.One
            }
        };

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

            try
            {
                var commandLine = new Cli.Parser();

                commandLine.Add(OptionNoBanner);
                commandLine.Add(OptionHelp);
                commandLine.Add(OptionProfileFile);
                commandLine.Add(OptionLogFile);
                commandLine.Add(OptionOutputPath);
                commandLine.Add(OptionTargetPlatform);
                commandLine.Add(OptionTargetToolchain);
                commandLine.Add(OptionGeneratorType);

                var parseResult = commandLine.Parse(args);

                if (parseResult.TryGet(OptionLogFile, out var logFilePath) && logFilePath != null)
                {
                    Trace.Listeners.Add(new TextWriterTraceListener(logFilePath.CreateText(), "LogFile"));
                }

                if (!parseResult.Has(OptionNoBanner))
                {
                    Application.PrintLogo();
                }

                Trace.WriteLine($@"Neobyte Build version {Application.Current}");
                Trace.WriteLine($@"{RuntimeInformation.OSDescription} ({RuntimeInformation.FrameworkDescription})");
                Trace.WriteLine($@"Working directory: {Environment.CurrentDirectory}");

                var unmatched = parseResult.Unmatched.FirstOrDefault();

                if (unmatched != null)
                {
                    throw new Exception($@"Unknown command line option ""{unmatched}""");
                }

                if (parseResult.Has(OptionHelp))
                {
                    Trace.WriteLine($@"Help:");
                    commandLine.Help(Console.Out);

                    foreach (var r in parseResult.Unmatched)
                    {
                        Trace.WriteLine(r);
                    }

                    return 0;
                }
                else
                {
                    var app = new Application(parseResult);
                    return app.Run();
                }
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
                Trace.Flush();
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
            Console.WriteLine("  / /| | / __ \\/ _ \\/ __ `__ \\/ __ \\/ __ \\/ _ \\  / __  / / / / / / __  /");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" / ___ |/ / / /  __/ / / / / / /_/ / / / /  __/ / /_/ / /_/ / / / /_/ /");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("/_/  |_/_/ /_/\\___/_/ /_/ /_/\\____/_/ /_/\\___(_)_____/\\__,_/_/_/\\__,_/");
            Console.WriteLine();

            Console.ForegroundColor = previous;
        }

        private static void ReportException(Exception? e)
        {
            while (e != null)
            {
                Trace.WriteLine($@"Failure: {e.Message}");

#if DEBUG
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
        private Profile? m_Profile;
        private TargetPlatform? m_TargetPlatform;
        private TargetToolchain? m_TargetToolchain;
        private DirectoryInfo? m_OutputDirectory;

        private Application(Cli.Result result)
        {
            var path = result.Get(OptionProfileFile);

            if (path != null && path.Exists)
            {
                using var fs = path.OpenRead();
                this.m_Profile = new Profile(fs.ReadAllBytes());
            }
            else
            {
                throw new Exception("Profile file not specified");
            }


            if (result.TryGet(OptionTargetPlatform, out var platform))
            {
                this.m_TargetPlatform = platform;
            }
            else
            {
                throw new Exception("Target platform not specified");
            }

            if (result.TryGet(OptionTargetToolchain, out var toolchain))
            {
                this.m_TargetToolchain = toolchain;
            }
            else
            {
                throw new Exception("Target platform not specified");
            }

            result.TryGet(OptionOutputPath, out this.m_OutputDirectory);
        }

        private int Run()
        {
            var targetPlatform = this.m_TargetPlatform!.Value;
            var targetToolchain = this.m_TargetToolchain!.Value;

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
                this.m_Profile!);

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
