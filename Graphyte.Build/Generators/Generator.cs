namespace Graphyte.Build.Generators
{
    public abstract class GeneratorBase
    {
        protected GeneratorBase()
        {
        }

        public abstract GeneratorType GeneratorType { get; }
    }
}
