using System.Collections.Generic;

namespace Neobyte.Build.Cli
{
    public sealed class Result
    {
        private readonly IReadOnlyDictionary<Option, object?> m_Values;
        public readonly string[] Unmatched;

        internal Result(
            IReadOnlyDictionary<Option, object?> values,
            string[] unmatched)
        {
            this.m_Values = values;
            this.Unmatched = unmatched;
        }

        public T? Get<T>(Option<T> option)
        {
            return (T?)this.m_Values[option];
        }

        public bool TryGet<T>(Option<T> option, out T? result)
        {
            if (this.m_Values.TryGetValue(option, out var temp))
            {
                result = (T?)temp;
                return true;
            }

            result = default;
            return false;
        }

        public bool Has(Option option)
        {
            return this.m_Values.ContainsKey(option);
        }
    }
}
