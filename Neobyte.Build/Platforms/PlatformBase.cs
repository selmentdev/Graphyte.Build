using Neobyte.Build.Framework;
using System;

namespace Neobyte.Build.Platforms
{
    public abstract class PlatformBase
    {
        protected PlatformBase(Profile profile, TargetArchitecture architecture)
        {
            this.Profile = profile;
            this.Architecture = architecture;
        }

        public Profile Profile { get; }

        public TargetArchitecture Architecture { get; }

        public abstract TargetPlatform Platform { get; }

        public abstract bool IsPlatformKind(TargetPlatformKind kind);

        public abstract string AdjustModuleName(string name, ModuleType type);

        public string[] IncludePaths { get; protected set; } = Array.Empty<string>();

        public string[] LibraryPaths { get; protected set; } = Array.Empty<string>();
    }
}
