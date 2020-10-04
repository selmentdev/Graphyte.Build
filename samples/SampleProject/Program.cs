using Graphyte.Build;

namespace SampleProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Graphyte.Build.Executor.Main(args);
        }
    }
}

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

            if (context.Configuration == ConfigurationType.Debug)
            {
                target.Runtime = RuntimeKind.Debug;
            }
            else
            {
                target.Runtime = RuntimeKind.Release;
            }

            target.Options.AddRange(new object[] {
                new Graphyte.Build.Toolsets.DisableSpecificWarnings("1412", "2321"),
                Graphyte.Build.Toolsets.Msvc.Compiler.InstructionSet.AVX,
                Graphyte.Build.Toolsets.Msvc.Compiler.RuntimeLibrary.MultiThreaded,
                new Graphyte.Build.Toolsets.Msvc.Linker.DelayLoadDlls("hello.dll"),
            });
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