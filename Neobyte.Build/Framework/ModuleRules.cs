using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Neobyte.Build.Framework
{
    public abstract class ModuleRules
    {
        public ModuleRules(TargetRules target)
        {
            var type = this.GetType();
            var location = type.GetCustomAttribute<ModuleRulesAttribute>();

            if (location is null)
            {
                throw new Exception($@"Module ""{type}"" must declare {nameof(ModuleRulesAttribute)}");
            }

            this.SourceFile = new FileInfo(location.Location);

            this.Guid = Core.Tools.MakeGuid(this.GetType().FullName);

            this.Target = target;
        }

        public TargetRules Target { get; }

        public FileInfo SourceFile { get; }
        public DirectoryInfo SourceDirectory => this.SourceFile.Directory;

        public Guid Guid { get; set; }

        public ModuleLanguage Language { get; protected init; }
        public ModuleType Type { get; protected init; }
        public ModuleKind Kind { get; protected init; }

        public List<Type> PublicDependencies { get; } = new();
        public List<Type> PrivateDependencies { get; } = new();
        public List<Type> InterfaceDependencies { get; } = new();

        public List<string> PublicIncludePaths { get; } = new();
        public List<string> PrivateIncludePaths { get; } = new();
        public List<string> InterfaceIncludePaths { get; } = new();

        public List<string> PublicLibraryPaths { get; } = new();
        public List<string> PrivateLibraryPaths { get; } = new();
        public List<string> InterfaceLibraryPaths { get; } = new();

        public List<string> PublicLibraries { get; } = new();
        public List<string> PrivateLibraries { get; } = new();
        public List<string> InterfaceLibraries { get; } = new();

        public List<string> PublicDefines { get; } = new();
        public List<string> PrivateDefines { get; } = new();
        public List<string> InterfaceDefines { get; } = new();

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
