namespace Graphyte.Build.Generators
{
    public abstract class BaseGenerator
    {
        public abstract bool IsHostSupported { get; }

        public abstract void Initialize(Profile profile);

        public abstract GeneratorType Type { get; }

        public abstract void PreConfigureTarget(Target target);

        public abstract void PostConfigureTarget(Target target);
    }
}
