using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Graphyte.Build
{
    [Serializable]
    public class CommandLineParsingException : Exception
    {
        public CommandLineParsingException()
        {
        }

        public CommandLineParsingException(string message)
            : base(message)
        {
        }

        public CommandLineParsingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CommandLineParsingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    public static class CommandLineParser
    {
        public static T Parse<T>(string[] args)
            where T : new()
        {
            //
            // Algorithm:
            //
            //  - get all fields and use their names as options
            //  - iterate over args and consume elements
            //
            //
            // Syntax:
            //
            //  -Param:Value
            //  -Param:"string value"
            //  -BooleanSwitch
            //

            var type = typeof(T);
            var fields = type.GetFields()
                .Where(x => x.IsPublic && !x.IsStatic && !x.IsInitOnly && !x.FieldType.IsArray)
                .ToArray();

            var options = new Dictionary<FieldInfo, string>();

            foreach (var arg in args)
            {
                var argument = arg.AsSpan();

                if (argument.Length <= 1)
                {
                    throw new CommandLineParsingException(arg);
                }

                if (argument[0] != '-')
                {
                    throw new CommandLineParsingException(arg);
                }

                // Remove '-' character
                var nameAndValue = argument.Slice(1);

                (var name, var value) = SplitNameValue(nameAndValue);

                var field = fields.FirstOrDefault(x => x.Name == name);

                if (field == null)
                {
                    throw new CommandLineParsingException($@"Command line argument ""{name}"" not found");
                }

                if (!options.TryAdd(field, value))
                {
                    throw new CommandLineParsingException($@"Command line argument ""{name}"" specified more than once");
                }
            }

            return ConstructParams<T>(options);
        }

        private static T ConstructParams<T>(IReadOnlyDictionary<FieldInfo, string> options)
            where T : new()
        {
            var processed = options.ToDictionary(
                x => x.Key,
                x =>
                {
                    (var field, var value) = x;

                    var type = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;

                    if (type.IsEnum)
                    {
                        return Enum.Parse(type, value);
                    }
                    else if (type == typeof(int))
                    {
                        return int.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else if (type == typeof(float))
                    {
                        return float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else if (type == typeof(bool))
                    {
                        return value switch
                        {
                            null => true,
                            _ => bool.Parse(value)
                        };
                    }
                    else if (type == typeof(string))
                    {
                        if (value.StartsWith('"') && value.EndsWith('"') && value.Length > 1)
                        {
                            return value[1..^1];
                        }
                        else
                        {
                            return value;
                        }
                    }
                    else if (type == typeof(FileInfo))
                    {
                        return new FileInfo(value);
                    }
                    else
                    {
                        throw new CommandLineParsingException($@"Type ""{type}"" is not supported");
                    }
                });

            var result = new T();

            foreach ((var field, var value) in processed)
            {
                field.SetValue(result, value);
            }

            return result;
        }

        private static (string, string) SplitNameValue(ReadOnlySpan<char> line)
        {
            var separator = line.IndexOfAny(':', '=');

            if (separator >= 0)
            {
                return (line.Slice(0, separator).ToString(), line.Slice(separator + 1).ToString());
            }
            else
            {
                return (line.ToString(), null);
            }
        }
    }
}
