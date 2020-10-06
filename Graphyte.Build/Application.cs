using System;
using System.Diagnostics;
using System.Runtime;

namespace Graphyte.Build
{
    public static class Application
    {
        private static readonly Version Current = new Version(1, 0, 0, 0);

        public static int Main(string[] args)
        {
            try
            {
                //
                // Initialize tracing.
                //

                Trace.Listeners.Add(new ConsoleTraceListener());
                Trace.WriteLine($@"Graphyte Build version {Application.Current}");

#if DEBUG
                for (var i = 0; i < args.Length; ++i)
                {
                    Debug.WriteLine($@"commandline[{i}] = ""{args[i]}""");
                }
#endif

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
                var profile = Graphyte.Build.Profiles.Profile.Parse(bytes);

                Trace.WriteLine($@"Platform: {profile.GetSection<Graphyte.Build.Platforms.BasePlatformSettings>().GetType().Name}");
                Trace.WriteLine($@"Toolchain: {profile.GetSection<Graphyte.Build.Toolchains.BaseToolchainSettings>().GetType().Name}");
                Trace.WriteLine($@"Generator: {profile.GetSection<Graphyte.Build.Generators.BaseGeneratorSettings>().GetType().Name}");
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