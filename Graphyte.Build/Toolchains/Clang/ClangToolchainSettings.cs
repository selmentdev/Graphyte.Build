
namespace Graphyte.Build.Toolchains.Clang
{
    public sealed class ClangToolchainSettings
        : BaseToolchainSettings
    {
        public bool AddressSanitizer { get; set; }
        public bool ThreadSanitizer { get; set; }
        public bool MemorySanitizer { get; set; }
        public bool UndefinedBehaviorSanitizer { get; set; }

        public bool TimeTrace { get; set; }

        public bool PgoOptimize { get; set; }
        public bool PgoProfile { get; set; }
        public string PgoDirectory { get; set; }
        public string PgoPrefix { get; set; }
    }
}
