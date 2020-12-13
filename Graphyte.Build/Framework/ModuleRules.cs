using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Graphyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ModuleRulesAttribute : Attribute
    {
        public readonly string Location;

        public ModuleRulesAttribute(
            [CallerFilePath] string location = "")
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
    public abstract class ModuleRules
    {
        public ModuleRules(TargetRules targetRules)
        {
            var type = this.GetType();
            var location = type.GetCustomAttribute<ModuleRulesAttribute>();

            if (location is null)
            {
                throw new Exception($@"Module ""{type}"" must declare {nameof(ModuleRulesAttribute)}");
            }

            this.ModuleFile = new FileInfo(location.Location);
            this.ModuleDirectory = this.ModuleFile.Directory;

            this.Guid = Core.Tools.MakeGuid(this.GetType().FullName);

            this.m_TargetRules = targetRules;
        }

        private readonly TargetRules m_TargetRules;

        public Guid Guid { get; set; }
        public FileInfo ModuleFile { get; }
        public DirectoryInfo ModuleDirectory { get; }
        public ModuleLanguage ModuleLanguage { get; protected init; }
        public ModuleType ModuleType { get; protected init; }
        public ModuleKind ModuleKind { get; protected init; }

        public List<Type> PublicDependencies { get; } = new List<Type>();
        public List<Type> PrivateDependencies { get; } = new List<Type>();
        public List<Type> InterfaceDependencies { get; } = new List<Type>();

        public List<string> PublicIncludePaths { get; } = new List<string>();
        public List<string> PrivateIncludePaths { get; } = new List<string>();
        public List<string> InterfaceIncludePaths { get; } = new List<string>();

        public List<string> PublicLibraryPaths { get; } = new List<string>();
        public List<string> PrivateLibraryPaths { get; } = new List<string>();
        public List<string> InterfaceLibraryPaths { get; } = new List<string>();

        public List<string> PublicLibraries { get; } = new List<string>();
        public List<string> PrivateLibraries { get; } = new List<string>();
        public List<string> InterfaceLibraries { get; } = new List<string>();

        public List<string> PublicDefines { get; } = new List<string>();
        public List<string> PrivateDefines { get; } = new List<string>();
        public List<string> InterfaceDefines { get; } = new List<string>();

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}