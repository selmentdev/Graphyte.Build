using System;

namespace Graphyte.Build.Framework
{
    public enum TargetType
    {
        /// <summary>
        /// Monolithic game executable. Contains both client and server code.
        /// </summary>
        Game,

        /// <summary>
        /// Modular game executable. Contains both client and server code.
        /// </summary>
        Editor,

        /// <summary>
        /// Monolithic game executable. Contains only client code.
        /// </summary>
        Client,

        /// <summary>
        /// Monolithic game executable. Contains only server code.
        /// </summary>
        Server,

        /// <summary>
        /// Modular executable. Contains both client and server code.
        /// </summary>
        Application,
    }

    public enum TargetLinkType
    {
        /// <summary>
        /// Choose default link type for given target type.
        /// </summary>
        Default,

        /// <summary>
        /// Target builds modular executables.
        /// </summary>
        Modular,

        /// <summary>
        /// Target builds monolithic executables.
        /// </summary>
        Monolithic,
    }

    public static partial class TargetExtensions
    {
        public static TargetLinkType GetDefaultTargetLinkType(this TargetType self)
        {
            switch (self)
            {
                case TargetType.Game:
                case TargetType.Client:
                case TargetType.Server:
                    return TargetLinkType.Monolithic;

                case TargetType.Application:
                case TargetType.Editor:
                    return TargetLinkType.Modular;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }

        public static TargetLinkType GetDefaultForTargetType(this TargetLinkType self, TargetType target)
        {
            if (self == TargetLinkType.Default)
            {
                return target.GetDefaultTargetLinkType();
            }

            return self;
        }
    }
}
