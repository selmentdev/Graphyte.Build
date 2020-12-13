namespace Graphyte.Build.Generators
{
    public abstract class Generator
    {
        protected Generator()
        {
        }

        public abstract GeneratorType GeneratorType { get; }
    }
}
