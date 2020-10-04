namespace Graphyte.Build.Toolsets
{
    public abstract class StringOption
    {
        public readonly string Value;

        public StringOption(string value)
        {
            this.Value = value;
        }
    }

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

    namespace Compiler
    {
        public enum TreatWarningAsError
        {
            Enable,
            Disable,
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
            AVX512,
            NEON,
        }

        public enum RuntimeLibrary
        {
            MultiThreaded,
            MultiThreadedDebug,
            MultiThreadedDLL,
            MultiThreadedDebugDLL,
        }

        public enum RuntimeTypeInfo
        {
            Enable,
            Disable,
        }

        public enum Permissive
        {
            Enable,
            Disable,
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
