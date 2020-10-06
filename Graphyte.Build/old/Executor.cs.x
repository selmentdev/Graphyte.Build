using Graphyte.Build.Platforms;
using Graphyte.Build.Platforms.Windows;
using Graphyte.Build.Resolving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime;

namespace Graphyte.Build
{
    public static class Executor
    {
        public static void Main(string[] _)
        {
            var stopwatch = Stopwatch.StartNew();

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());

            var profile = new Profile()
            {
                Platform = PlatformType.Windows,
                Architecture = ArchitectureType.X64,
            };

            File.WriteAllText("win64.json", ProfileSerializer.Serialize(profile));

            // Need inputs:
            //      - platform type
            //      - architecture type
            //      - toolset type
            //      - build type
            //      - configuration type

            var targetTuples = new TargetTuple[]
            {
                new TargetTuple("Windows-X64-MSVC"),
            };

            var platformProvider = new PlatformProvider();
            var visualStudioProvider = new VisualStudioProvider();
            var windowsSdkProvider = new WindowsSdkProvider();
            var solutionProvider = new SolutionProvider();

            var resolvedSolutions = new List<ResolvedSolution>();

            foreach (var solution in solutionProvider.GetSolutions())
            {
                foreach (var tuple in targetTuples)
                {
                    foreach (var build in solution.BuildTypes)
                    {
                        foreach (var config in solution.ConfigurationTypes)
                        {
                            var context = new Context(
                                tuple.Platform,
                                tuple.Architecture,
                                tuple.Toolset,
                                build,
                                config);

                            var resolvedSolution = new ResolvedSolution(solution, context);

                            resolvedSolution.Resolve();

                            resolvedSolutions.Add(resolvedSolution);
                        }
                    }
                }
            }

            Dump.DumpResolvedSolution.SaveToFile("test.json", resolvedSolutions);

#if DEBUG
            Console.WriteLine("# Discovered Platforms");

            foreach (var platform in platformProvider.GetPlatforms())
            {
                Console.WriteLine($@"  - name: `{platform.Name}`, is host supported: `{platform.IsHostSupported}`");
            }

            Console.WriteLine("# Visual Studio");

            foreach (var instance in visualStudioProvider.Instances)
            {
                Console.WriteLine($@"  - name: `{instance.Name}`, version: `{instance.Version}`, location: `{instance.Location}`, toolkit: `{instance.Toolkit}`, toolset: `{instance.Toolset}`");
            }

            Console.WriteLine("# Windows SDK");
            Console.WriteLine($@"  - is supported: `{windowsSdkProvider.IsSupported}`, location: `{windowsSdkProvider.Location}`");

            foreach (var sdk in windowsSdkProvider.Versions)
            {
                Console.WriteLine($@"  - version: `{sdk}`");
            }

            Console.WriteLine("# Solutions");

            foreach (var instance in solutionProvider.GetSolutions())
            {
                Console.WriteLine($@"  - {instance.Name}");
            }
#endif

            LogMemoryUsage();

            Trace.WriteLine($@"  duration: {stopwatch.Elapsed.TotalSeconds:0.0.00} seconds");
            Trace.WriteLine($@"  completed: {DateTime.Now}");
        }

        private static void LogMemoryUsage()
        {
            using var proc = Process.GetCurrentProcess();
            Console.WriteLine($@"Used memory: {proc.PrivateMemorySize64 / (1024 * 1024)} MiB");
        }
    }
}
