namespace Neobyte.Build.Cli
{
    public class Option<T> : Option
    {
        public Option(string name, string? description = null)
            : base(name, typeof(T))
        {
            this.Argument = new Argument<T>();
            this.Description = description;
        }
    }
}
