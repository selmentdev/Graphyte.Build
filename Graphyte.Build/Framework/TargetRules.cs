using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Graphyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TargetRulesAttribute
        : Attribute
    {
        public readonly string Location;

        public TargetRulesAttribute([CallerFilePath] string location = "")
        {
            this.Location = location;
        }

        public override string ToString()
        {
            return this.Location;
        }
    }
}

namespace Graphyte.Build.Framework
{
    /// <summary>
    /// Describes application target
    /// </summary>
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
