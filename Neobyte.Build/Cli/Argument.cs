using System;

namespace Neobyte.Build.Cli
{
    public class Argument
    {
        private Type m_ArgumentType = typeof(string);

        public Type ArgumentType
        {
            get => this.m_ArgumentType;
            set => this.m_ArgumentType = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Name { get; protected set; }
        public string? Description { get; protected set; }

        public ArgumentArity Arity { get; set; }

        public static readonly Argument None = new() { Arity = ArgumentArity.Zero };

        public Argument()
        {
            this.Name = string.Empty;
        }

        public Argument(string name)
        {
            this.Name = name;
        }
    }
}
