        [DefaultValue(false)]
        public bool? EnableMemoryTracing { get; set; }

        [DefaultValue(false)]
        public bool? EnableProfiling { get; set; }

        public string ProductName { get; set; }
        public string CompanyName { get; set; }
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
