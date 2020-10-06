using System;

namespace Graphyte.Build
{
    public enum TargetType
    {
        /// <summary>
        /// Platform decides how to handle compiled libraries.
        /// </summary>
        /// <remarks>
        /// Depending on build configuration, this type may be expanded to SharedLibrary or
        /// StaticLibrary.
        /// </remarks>
        Default,

        /// <summary>
        /// Target is linked as shared library.
        /// </summary>
        SharedLibrary,

        /// <summary>
        /// Target is linked as static library.
        /// </summary>
        StaticLibrary,

        /// <summary>
        /// Target does not produce any compiler library, but may provide external dependencies.
        /// </summary>
        HeaderLibrary,

        /// <summary>
        /// Target is an application.
        /// </summary>
        /// <remarks>
        /// To specify if application is an console application, please use target rules instance.
        /// </remarks>
        Application,
    }

    public static class TargetTypeExtensions
    {
        public static bool IsImportable(this TargetType self)
        {
            switch (self)
            {
                case TargetType.Default:
                    break;

                case TargetType.StaticLibrary:
                case TargetType.HeaderLibrary:
                    return true;

                case TargetType.SharedLibrary:
                case TargetType.Application:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }

        public static bool IsLibrary(this TargetType self)
        {
            switch (self)
            {
                case TargetType.Default:
                    break;

                case TargetType.SharedLibrary:
                case TargetType.StaticLibrary:
                case TargetType.HeaderLibrary:
                    return true;

                case TargetType.Application:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }

        public static bool IsApplication(this TargetType self)
        {
            switch (self)
            {
                case TargetType.Default:
                    break;

                case TargetType.SharedLibrary:
                case TargetType.StaticLibrary:
                case TargetType.HeaderLibrary:
                    return false;

                case TargetType.Application:
                    return true;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }
}
