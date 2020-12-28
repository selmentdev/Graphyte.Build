using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Neobyte.Build.Cli
{
    public sealed class Parser
    {
        private HashSet<Option> m_Options = new();

        public Parser()
        {
        }

        public void Add(Option option)
        {
            this.m_Options.Add(option);
        }

        public Result Parse(string[] args)
        {
            // command
            // command [--option]
            // command [--option [<argument>...] ...]

            var result = new Dictionary<Option, object?>();

            var items = args.AsSpan();

            while (items.Length > 0)
            {
                var name = items[0];

                var option = this.m_Options.FirstOrDefault(x => x.Name == name);
                if (option != null)
                {
                    if (result.ContainsKey(option))
                    {
                        throw new Exception($@"Option ""{name}"" specified more than once");
                    }

                    // take tail as possible number of elements
                    var tail = items.Slice(1);

                    var argument = option.Argument;

                    if (tail.Length < argument.Arity.Min)
                    {
                        throw new Exception($@"Not enough arguments for option {option.Name}");
                    }

                    var maxArgs = Math.Min(argument.Arity.Max, tail.Length);

                    var values = tail.Slice(0, maxArgs);

                    if (values.Length == 0)
                    {
                        result.Add(option, null);
                    }
                    else if (values.Length == 1)
                    {
                        result.Add(option, ValueConverter.FromString(argument.ArgumentType, values[0]));
                    }
                    else
                    {
                        throw new NotSupportedException($@"Arity of [{argument.Arity.Min}..{argument.Arity.Max}) is not supported");
                    }

                    items = tail.Slice(maxArgs);

                    continue;
                }

                // Couldn't match anything here
                break;
            }

            return new Result(
                values: result,
                unmatched: items.ToArray());

        }

        public void Help(TextWriter writer)
        {
            var options = this.m_Options
                .Select(x => (Name: GetDisplayString(x), x.Description))
                .OrderBy(x => x.Name)
                .ToArray();

            foreach (var (name, description) in options)
            {
                if (string.IsNullOrEmpty(description))
                {
                    writer.WriteLine($@"  {name,-40}");
                }
                else
                {
                    writer.WriteLine($@"  {name,-40} {description}");
                }
            }
        }

        private static string GetDisplayString(Option option)
        {
            if (string.IsNullOrEmpty(option.Argument.Name))
            {
                return $@"{option.Name}";
            }
            else
            {
                return $@"{option.Name} <{option.Argument.Name}>";
            }
        }
    }
}
