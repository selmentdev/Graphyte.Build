using Neobyte.Build.Platforms;
using System.Collections.Generic;
using System.Linq;

namespace Neobyte.Build.Framework
{
    // Design consideration:
    //  - current approach allows us to build release dev tools even for debug target (as a dependency)
    //      - developer tools required for building other source code may run as fast as possible
    //      - this means that target may have dependency as well
    //  - sharing resolved modules within common workspace will prevent that
    //
    // Decision:
    //  - leave targets as-is? even if they are shared, we may still generate separate binaries
    //  - allow sharing; modules built for single target may be placed side-by-side
    //  - target dependencies are handled separately; build system knows where release tools are placed, so moving them to separate directory is not a problem.

    public sealed class Workspace
    {
        public IEnumerable<TargetRulesFactory> Targets { get; }

        public IEnumerable<ModuleRulesFactory> Modules { get; }

        public IEnumerable<PlatformFactory> Platforms { get; }

        public IEnumerable<TargetFlavor> Flavors { get; set; } = TargetExtensions.Flavors;

        public IEnumerable<TargetConfiguration> Configurations { get; set; } = TargetExtensions.Configurations;

        public Profile Profile { get; }

        public Workspace(
            IEnumerable<TargetRulesFactory> targets,
            IEnumerable<ModuleRulesFactory> modules,
            IEnumerable<PlatformFactory> platforms,
            Profile profile)
        {
            this.Targets = targets.ToArray();
            this.Modules = modules.ToArray();
            this.Platforms = platforms.ToArray();
            this.Profile = profile;
        }
    }
}
