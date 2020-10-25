namespace Graphyte.Build.Toolchains
{
    public abstract class BaseToolchain
    {
        public abstract bool IsHostSupported { get; }

        public abstract ToolchainType Type { get; }

        public abstract void Initialize(Profile profile);

        public abstract void PreConfigureTarget(Target target);

        public abstract void PostConfigureTarget(Target target);
    }
}
