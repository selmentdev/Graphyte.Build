namespace Graphyte.Build.Platforms
{
    public abstract class BasePlatform
    {
        public abstract bool IsHostSupported { get; }

        public abstract bool IsSupported(TargetTuple tuple);

        public virtual void PreConfigureTarget(Target target, IContext context)
        {
        }

        public virtual void PostConfigureTarget(Target target, IContext context)
        {
        }

        public abstract string AdjustTargetName(string name, TargetType targetType);

        public string Name => this.GetType().Name;
    }
}
