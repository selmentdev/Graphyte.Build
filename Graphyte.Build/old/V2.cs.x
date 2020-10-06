    [Serializable]
    public class Profile
    {
        public string Prefix { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Platform Platform { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Architecture Architecture { get; set; }

        public Architecture[] Architectures { get; set; } = new[]
        {
            Architecture.X64,
            Architecture.X86,
            Architecture.ARM64,
        };

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Compiler Compiler { get; set; }


        [DefaultValue(false)]
        public bool? EnableAddressSanitizer { get; set; }

        [DefaultValue(false)]
        public bool? EnableThreadSanitizer { get; set; }

        [DefaultValue(false)]
        public bool? EnableUndefinedBehaviorSanitizer { get; set; }

        [DefaultValue(false)]
        public bool? EnableUnityBuild { get; set; }

        [DefaultValue(false)]
        public bool? EnableDistributedBuild { get; set; }

        [DefaultValue(false)]
        public bool? EnableMemoryTracing { get; set; }

        [DefaultValue(false)]
        public bool? EnableProfiling { get; set; }

        public bool? EnableStaticAnalyzer { get; set; }

        public bool? EnableClangTimeTrace { get; set; }

        public string ProductName { get; set; }
        public string CompanyName { get; set; }

        [DefaultValue(false)]
        public bool? PgoOptimize { get; set; }

        [DefaultValue(false)]
        public bool? PgoProfile { get; set; }

        public string PgoDirectory { get; set; }

        public string PgoPrefix { get; set; }
    }
}

// Solutions and Projects
namespace Graphyte.V2.Build
{
    public interface ICompilerContext
    {
    }

    public sealed class CppCompilerContext : ICompilerContext
    {
        public bool EnableRtti = false;
        public bool EnableExceptions = false;
        public bool EnableAvx = false;
        public bool EnableInlining = false;
        public bool EnableSecurityChecks = true;
        public bool EnableUnityBuild = true;
        public bool OptimizeCode = false;
        public bool OptimizeForSize = false;
        public bool UseStaticRuntime;
        public bool UseDebugRuntime;
        public bool CreateDebugInfo;
        public bool IsCompilingLibrary;
        public bool OmitFramePointers;
        public bool UsePdbFiles;
        public bool EnableIncrementalLinking;
        public bool EnableLinkTimeCodeGeneration;
    }

    public interface IPlatform
    {
        void PreConfigure(Target target);
        void PostConfigure(Target target);

        void Configure(CppCompilerContext context);
    }

    public interface IToolchain
    {
        void PreConfigure(Target target);
        void PostConfigure(Target target);

        void Configure(CppCompilerContext context);
    }
}
