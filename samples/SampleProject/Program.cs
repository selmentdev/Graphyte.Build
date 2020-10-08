using Graphyte.Build;
using System.Diagnostics;

namespace SampleProject
{
    public sealed class MyApp : Project
    {
        public MyApp()
        {

        }

        public override void Configure(Target target)
        {
            target.ComponentType = ComponentType.GameApplication;
            target.TargetType = TargetType.Application;
        }
    }

    public sealed class MySolution : Solution
    {
        public MySolution()
        {
            this.AddProject(new MyApp());
        }
    }

    internal class Program
    {
        private static int Main(string[] args)
        {
            return Application.Main(args);
        }
    }
}
