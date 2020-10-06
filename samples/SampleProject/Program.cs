using System;
using System.Diagnostics;

namespace SampleProject
{
    [Serializable]
    public class WindowsPlatformSettings : Graphyte.Build.PlatformSettings
    {
        public string WindowsSdkVersion { get; set; }
    }

    [Serializable]
    public class XxxPlatformSettings : Graphyte.Build.PlatformSettings
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }

    [Serializable]
    public class XxxGeneratorSettings : Graphyte.Build.GeneratorSettings
    {
        public string CachePath { get; set; }
    }

    internal class Program
    {
        private static int Main(string[] args)
        {
            Graphyte.Build.Application.Main(args);

            var settings = Graphyte.Build.Settings.Parse(@"
{
    ""XxxPlatformSettings"": {
        ""StringProperty"": ""value"",
        ""IntProperty"": 42,
    },
    ""WindowsPlatformSettings"": {
        ""WindowsSdkVersion"": ""10.0.10240.0"",
        ""UnknownProperty"": 42,
    },
    ""XxxGeneratorSettings"": {
        ""CachePath"": ""some/other/path"",
    },
}
");
            {
                var generators = settings.GetAll<Graphyte.Build.GeneratorSettings>();

                foreach (var generator in generators)
                {
                    Trace.WriteLine($@"- generator: {generator.GetType().Name}");
                }
            }

            {
                var platforms = settings.GetAll<Graphyte.Build.PlatformSettings>();

                foreach (var platform in platforms)
                {
                    Trace.WriteLine($@"- platform: {platform.GetType().Name}");
                }
            }

            return 0;
        }
    }
}

#if false
namespace SampleProject
{
    public class SampleSolution : Solution
    {
        public SampleSolution()
        {
            this.AddBuildType(BuildType.Developer);
            this.AddBuildType(BuildType.Retail);
            this.AddBuildType(BuildType.Testing);

            this.AddConfigurationType(ConfigurationType.Debug);
            this.AddConfigurationType(ConfigurationType.Release);

            this.AddProject(new MainExecutable());
            this.AddProject(new BaseLibrary());
            this.AddProject(new SharedLibrary());
        }
    }

    public class MainExecutable : Project
    {
        public override void Configure(Target target, IContext context)
        {
            target.AddPublicDependency<BaseLibrary>();
            target.AddPrivateDependency<SharedLibrary>();

            target.Runtime = context.Configuration
                == ConfigurationType.Debug
                ? RuntimeKind.Debug
                : RuntimeKind.Release;

            target.Options.AddRange(new object[] {
                (Graphyte.Build.Toolsets.Msvc.Linker.Subsystem)0,
                new Graphyte.Build.Toolsets.DisableSpecificWarnings("1412", "2321"),
                Graphyte.Build.Toolsets.Msvc.Compiler.InstructionSet.AVX,
                Graphyte.Build.Toolsets.Msvc.Compiler.RuntimeLibrary.MultiThreaded,
                new Graphyte.Build.Toolsets.Msvc.Linker.DelayLoadDlls("hello.dll"),
            });

            var sb = new StringBuilder();
            MsvcOptionsDispatcher.HandleOptions(sb, target.Options);

            //MsvcOptionsDispatcher.Experimental(sb, target.Options);
            Debug.WriteLine(sb.ToString());
        }
    }

    public class BaseLibrary : Project
    {
        public override void Configure(Target target, IContext context)
        {
        }
    }

    public class SharedLibrary : Project
    {
        public override void Configure(Target target, IContext context)
        {
        }
    }
}
#endif