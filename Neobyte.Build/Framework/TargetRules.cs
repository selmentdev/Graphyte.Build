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
            this.SourceDirectory = this.SourceFile.Directory;
        }

        public FileInfo SourceFile { get; }

        public DirectoryInfo SourceDirectory { get; }

        public TargetDescriptor Descriptor { get; }

        public TargetContext Context { get; }

        public TargetType Type { get; protected set; } = TargetType.Game;

        public TargetLinkType LinkType { get; protected set; } = TargetLinkType.Monolithic;

        public Type LaunchModule { get; protected set; }

        /// <summary>
        /// Gets list of modules required to build this target.
        /// </summary>
        public List<Type> Modules { get; } = new List<Type>();

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
