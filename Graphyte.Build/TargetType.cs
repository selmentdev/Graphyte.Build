using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public enum TargetType
    {
        /// <summary>
        /// Compiles project source code as shared library.
        /// </summary>
        SharedLibrary,

        /// <summary>
        /// Compiles project source code as static library.
        /// </summary>
        StaticLibrary,

        /// <summary>
        /// Specifies headers only library. Target is not compiled but its used as dependency for other projects.
        /// </summary>
        HeaderLibrary,

        /// <summary>
        /// Compiles project as executable.
        /// </summary>
        Application,
    }

    public static class TargetTypeExtensions
    {
        public static bool IsLibrary(this TargetType type)
        {
            switch (type)
            {
                case TargetType.SharedLibrary:
                case TargetType.StaticLibrary:
                case TargetType.HeaderLibrary:
                    return true;
                case TargetType.Application:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }

        public static bool IsApplication(this TargetType type)
        {
            switch (type)
            {
                case TargetType.SharedLibrary:
                case TargetType.StaticLibrary:
                case TargetType.HeaderLibrary:
                    return false;
                case TargetType.Application:
                    return true;
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
