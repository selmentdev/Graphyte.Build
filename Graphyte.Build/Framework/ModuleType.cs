using System;

namespace Graphyte.Build.Framework
{
    public enum ModuleType
    {
        // Indicates that user did not set proper value.
        Default,
        SharedLibrary,
        StaticLibrary,
        ExternLibrary,
        Application,
    }
}

namespace Graphyte.Build.Framework
{
    public static partial class ModuleExtensions
    {
        public static bool IsImportable(this ModuleType self)
        {
            switch (self)
            {
                case ModuleType.Default:
                    break;

                case ModuleType.StaticLibrary:
                case ModuleType.ExternLibrary:
                    return true;

                case ModuleType.SharedLibrary:
                case ModuleType.Application:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }

        public static bool IsLibrary(this ModuleType self)
        {
            switch (self)
            {
                case ModuleType.Default:
                    break;

                case ModuleType.SharedLibrary:
                case ModuleType.StaticLibrary:
                case ModuleType.ExternLibrary:
                    return true;

                case ModuleType.Application:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }

        public static bool IsApplication(this ModuleType self)
        {
            switch (self)
            {
                case ModuleType.Default:
                    break;

                case ModuleType.Application:
                    return true;

                case ModuleType.SharedLibrary:
                case ModuleType.StaticLibrary:
                case ModuleType.ExternLibrary:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }
}
