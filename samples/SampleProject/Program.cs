using Graphyte.Build;
using Graphyte.Build.Toolsets;
using System.Diagnostics;
using System.Text;

namespace SampleProject
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return Graphyte.V2.Build.Executor.Main(args);
            //Graphyte.Build.Executor.Main(args);
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