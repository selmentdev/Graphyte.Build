namespace Graphyte.Build.Platforms
{
    public abstract class BasePlatform
    {
        public abstract bool IsHostSupported { get; }
        public abstract bool IsSupported(TargetTuple targetTuple);

        public virtual void PreConfigureTarget(Target traget)
        {
        }

        public virtual void PostConfigureTarget(Target target)
        {
        }

        public abstract string AdjustTargetName(string name, TargetType targetType);

        public string Name => this.GetType().Name;
    }
}
