using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public enum TargetType
    {
        SharedLibrary,
        StaticLibrary,
        HeaderLibrary,
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
