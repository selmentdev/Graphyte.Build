namespace Graphyte.Build.Toolsets
{
    public abstract class StringListOption
    {
        public readonly string[] Values;

        public StringListOption(params string[] values)
        {
            this.Values = values;
        }
    }

    public class DisableSpecificWarnings : StringListOption
    {
        public DisableSpecificWarnings(params string[] values)
            : base(values)
        {
        }
    }
}

namespace Graphyte.Build.Toolsets.Msvc
{
    namespace Compiler
    {
        public enum InstructionSet
        {
            Disable,
            None,
            SSE2,
            AVX,
            AVX2,
            NEON,
        }

        public enum RuntimeLibrary
        {
            MultiThreaded,
            MultiThreadedDebug,
            MultiThreadedDLL,
            MultiThreadedDebugDLL,
        }
    }

    namespace Linker
    {
        public class DelayLoadDlls : StringListOption
        {
            public DelayLoadDlls(params string[] values)
                : base(values)
            {
            }
        }

        public enum GenerateDebugInformation
        {
            Enable,
            EnableFastlink,
            Disable,
        }

        public enum Subsystem
        {
            Console,
            Application,
            Native,
        }
    }

    namespace Librarian
    {

    }

    namespace LLVM
    {
        public enum UseClangCl
        {
            Enable,
            Disable,
        }

        public enum UseLldLink
        {
            Enable,
            Disable,
        }
    }
}

namespace Graphyte.Build.Toolsets.Clang
{
}
