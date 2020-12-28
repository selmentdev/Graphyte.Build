namespace Neobyte.Build.Cli
{
    public class Argument<T> : Argument
    {
        public Argument()
            : base()
        {
            this.ArgumentType = typeof(T);
        }

        public Argument(string name, string? description = null)
            : base(name)
        {
            this.ArgumentType = typeof(T);
            this.Description = description;
        }
    }
}
