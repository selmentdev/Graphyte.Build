using System;
using System.Linq;

namespace Graphyte.Build
{
    public static class PlatformExtensions
    {
        private static readonly Platform[] g_DesktopPlatforms = new[]
        {
            Platform.Windows,
            Platform.UWP,
            Platform.MacOS,
            Platform.Linux,
        };

        private static readonly Platform[] g_ConsolePlatforms = new[]
        {
            Platform.NX,
            Platform.Orbis,
            Platform.Prospero,
            Platform.XboxOne,
            Platform.Scarlett,
        };

        private static readonly Platform[] g_MobilePlatforms = new[]
        {
            Platform.Android,
            Platform.IOS,
        };

        private static readonly Platform[] g_ServerPlatforms = new[]
        {
            Platform.Windows,
            Platform.Linux,
        };

        public static Platform[] GetPlatforms(PlatformKind kind)
        {
            switch (kind)
            {
                case PlatformKind.Desktop:
                    return g_DesktopPlatforms;
                case PlatformKind.Console:
                    return g_ConsolePlatforms;
                case PlatformKind.Mobile:
                    return g_MobilePlatforms;
                case PlatformKind.Server:
                    return g_ServerPlatforms;
            }

            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        public static bool IsKindOf(this Platform self, PlatformKind kind)
        {
            var platforms = PlatformExtensions.GetPlatforms(kind);
            return platforms.Contains(self);
        }
    }
}
