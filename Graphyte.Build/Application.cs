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


            //
            // Report results.
            //

            watch.Stop();

            Trace.WriteLine($@"excution time: {watch.Elapsed.TotalSeconds:0.0.00}");
            return 0;
        }
    }
}