using Graphyte.Build.Framework;

namespace Graphyte.Build.Platforms
{
    public abstract class Platform
    {
        protected Platform(Profile profile, TargetArchitecture targetArchitecture)
        {
            this.Profile = profile;
            this.TargetArchitecture = targetArchitecture;
        }

        public Profile Profile { get; }

        public TargetArchitecture TargetArchitecture { get; }

        public abstract TargetPlatform TargetPlatform { get; }

        public abstract bool IsPlatformKind(TargetPlatformKind targetPlatformKind);

        public abstract string AdjustModuleName(string name, ModuleType moduleType);

        public string[] IncludePaths { get; protected set; }

        public string[] LibraryPaths { get; protected set; }
    }
}
