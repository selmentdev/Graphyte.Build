using System;

namespace Neobyte.Build.Cli
{
    public readonly struct ArgumentArity
    {
        public readonly int Min;
        public readonly int Max;

        public ArgumentArity(int min, int max)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException(nameof(min));
            }

            this.Min = min;
            this.Max = max;
        }

        internal const int MaxArity = 1_000;

        public static readonly ArgumentArity Zero = new(0, 0);
        public static readonly ArgumentArity ZeroOrOne = new(0, 1);
        public static readonly ArgumentArity One = new(0, 1);
        public static readonly ArgumentArity ZeroOrMore = new(0, MaxArity);
        public static readonly ArgumentArity OneOrMore = new(1, MaxArity);
    }
}
