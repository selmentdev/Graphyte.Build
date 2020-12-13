namespace Graphyte.Build.Framework
{
    public enum ModuleKind
    {
        // Indicates that user did not set proper value.
        Default,
        Developer,
        Runtime,
        Editor,
        Test,
        ThirdParty,
    }
}
