using System;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Graphyte.Build
{
    public static class Application
    {
        private static readonly Version Current = new Version(1, 0, 0, 0);

        public sealed class CommandLineParams
        {
            public FileInfo Profile;
            public Platform? Platform;
            public Configuration? Configuration;
            public Architecture? Architecture;
        }

        public static int Main(string[] args)
        {
            try
            {
                //
                // Initialize tracing.
                //

                Trace.Listeners.Add(new ConsoleTraceListener());
                Trace.WriteLine($@"Graphyte Build version {Application.Current}");
                Trace.WriteLine($@"{RuntimeInformation.OSDescription} ({RuntimeInformation.FrameworkDescription})");


                var options = CommandLineParser.Parse<CommandLineParams>(args);

                Debug.WriteLine($@"OPTIONS: profile = {options.Profile}");
                Debug.WriteLine($@"OPTIONS: platform = {options.Platform}");
                Debug.WriteLine($@"OPTIONS: configuration = {options.Configuration}");
                Debug.WriteLine($@"OPTIONS: architecture = {options.Architecture}");

#if DEBUG
                foreach (var arg in args)
                {
                    Debug.WriteLine($@"COMMANDLINE: ""{arg}""");
                }
#endif

                if (options.Profile != null)
                {
                    if (!options.Profile.Exists)
                    {
                        Trace.WriteLine($@"Profile file ""{options.Profile.FullName}"" does not exist");
                        return -1;
                    }
                }
                else if (options.Profile == null)
                {
                    Trace.WriteLine(@"Profile file not specified");
                    return -1;
                }
                else if (!options.Platform.HasValue)
                {
                    Trace.WriteLine(@"Platform not specified");
                    return -1;
                }
                else if (!options.Configuration.HasValue)
                {
                    Trace.WriteLine(@"Configuration not specified");
                    return -1;
                }
                else if (!options.Architecture.HasValue)
                {
                    Trace.WriteLine(@"Architecture not specified");
                    return -1;
                }

                //
                // Start timing.
                //

                var watch = Stopwatch.StartNew();


                //
                // Setup GC
                //

                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;


                var bytes = System.IO.File.ReadAllBytes(args[0]);
                var profile = Graphyte.Build.Profile.Parse(bytes);

                Trace.WriteLine($@"Platform: {profile.GetSection<Graphyte.Build.Platforms.BasePlatformSettings>().GetType().Name}");
                Trace.WriteLine($@"Toolchain: {profile.GetSection<Graphyte.Build.Toolchains.BaseToolchainSettings>().GetType().Name}");
                Trace.WriteLine($@"Generator: {profile.GetSection<Graphyte.Build.Generators.BaseGeneratorSettings>().GetType().Name}");

                var solutions = new SolutionProvider();

                Trace.WriteLine("Discovering solutions");
                Trace.Indent();
                foreach (var solution in solutions.GetSolutions())
                {
                    Trace.WriteLine($@"{solution.Name}");
                }
                Trace.Unindent();
                Trace.WriteLine("Done.");

                var platforms = new Platforms.PlatformProvider();

                Trace.WriteLine("Discovering platforms...");
                Trace.Indent();
                foreach (var platform in platforms.GetPlatforms())
                {
                    Trace.WriteLine($@"{platform.Name} (is supported: {platform.IsHostSupported})");
                }
                Trace.Unindent();
                Trace.WriteLine("Done.");

                Trace.WriteLine("Git Information");
                Trace.Indent();
                Trace.WriteLine($@"Commit-Long:  ""{Git.GetCommitId()}""");
                Trace.WriteLine($@"Commit-Short: ""{Git.GetCommitIdShort()}""");
                Trace.WriteLine($@"Branch:       ""{Git.GetBranchName()}""");
                Trace.Unindent();
                Trace.WriteLine("Done.");


                //
                // Report results.
                //

                watch.Stop();

                Trace.WriteLine($@"excution time: {watch.Elapsed.TotalSeconds:0.0.00}");
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Failed with exception: {e.Message}");
                return -1;
            }
        }
    }
}
