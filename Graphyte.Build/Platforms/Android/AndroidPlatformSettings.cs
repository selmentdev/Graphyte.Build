namespace Graphyte.Build.Platforms.Android
{
    public sealed class AndroidPlatformSettings
        : BasePlatformSettings
    {
        /// <summary>
        /// Provides path to Android SDK.
        /// </summary>
        public string SdkPath { get; set; }

        /// <summary>
        /// Provides path to Anrdoid NDK.
        /// </summary>
        public string NdkPath { get; set; }

        public int TargetApiLevel { get; set; }
    }
}
