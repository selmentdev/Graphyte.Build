using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Neobyte.Build.Framework
{
    /// <summary>
    /// Describes single target.
    /// </summary>
    /// <remarks>
    /// Target rules provide engine configuration for given list of modules.
    /// </remarks>
    public abstract class TargetRules
    {
        public TargetRules(TargetDescriptor descriptor, TargetContext context)
        {
            this.Descriptor = descriptor;
            this.Context = context;

            var type = this.GetType();
            var location = type.GetCustomAttribute<TargetRulesAttribute>();

            if (location is null)
            {
                throw new Exception($@"Target ""{type}"" must declare {nameof(TargetRulesAttribute)}");
            }

            this.SourceFile = new FileInfo(location.Location);
        }

        public FileInfo SourceFile { get; }

        public DirectoryInfo SourceDirectory => this.SourceFile.Directory;

        public TargetDescriptor Descriptor { get; }

        public TargetContext Context { get; }

        public TargetType Type { get; protected set; } = TargetType.Game;

        public TargetLinkType LinkType { get; protected set; } = TargetLinkType.Monolithic;

        /// <summary>
        /// Gets or sets module used when target is being executed.
        /// </summary>
        public Type LaunchModule { get; protected set; }

        /// <summary>
        /// Gets list of additional modules.
        /// </summary>
        /// <remarks>
        /// This list must include additional modules required to launch this target, but not
        /// explicitely defined as dependencies. Example of such modules are dynamically discovered
        /// plugins which are not implicit dependencies of launch module.
        /// </remarks>
        public List<Type> Modules { get; } = new();

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
