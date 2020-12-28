using System;

namespace Neobyte.Build.Cli
{
    // Argument: expects one or more values.
    // Example: `--an-argument 42`

    public abstract class Option
    {
        public string Name { get; }
        public Type Type { get; }

        public string? Description { get; set; }

        protected Option(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        private Argument? m_Argument;

        public Argument Argument
        {
            get => this.m_Argument ?? Argument.None;
            set => this.m_Argument = value;
        }
    }
}
