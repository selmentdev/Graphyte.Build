namespace Graphyte.Build
{
    /// <summary>
    /// Specifies available module types.
    /// </summary>
    public enum ModuleType
    {
        /// <summary>
        /// Developer module.
        /// </summary>
        /// <remarks>
        /// Module used to develop game.
        /// </remarks>
        Developer,

        /// <summary>
        /// Runtime module.
        /// </summary>
        /// <remarks>
        /// Implements common game and editor functionalities.
        /// </remarks>
        Runtime,

        /// <summary>
        /// Editor module.
        /// </summary>
        /// <remarks>
        /// Implements editor module. Not shippable.
        /// </remarks>
        Editor,

        /// <summary>
        /// Third party SDK.
        /// </summary>
        /// <remarks>
        /// External library.
        /// </remarks>
        ThirdPartySdk,

        /// <summary>
        /// Unit test application or module.
        /// </summary>
        UnitTest,
    }
}
