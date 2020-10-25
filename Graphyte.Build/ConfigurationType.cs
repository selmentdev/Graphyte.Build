namespace Graphyte.Build
{
    public enum ConfigurationType
    {
        /// <summary>
        /// Game and engine code are not optimized.
        /// </summary>
        Debug,

        /// <summary>
        /// Game code is not optimized, engine is optimized.
        /// </summary>
        DebugGame,

        /// <summary>
        /// Engine is optimized, game code is mostly optimized.
        /// </summary>
        Development,

        /// <summary>
        /// Release configuration with developer tools enabled.
        /// </summary>
        Testing,

        /// <summary>
        /// Game and engine are optimized.
        /// </summary>
        Release,
    }

    public enum ConfigurationFlavour
    {
        None,
        Editor,
        Client,
        Server,
    }
}
