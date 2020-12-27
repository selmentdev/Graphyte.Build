using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Core
{
    internal static class CommandLineValueConverter
    {
        private static ConstructorInfo? TryGetConstructor(this Type self, Type type)
        {
            return self.GetConstructors().SingleOrDefault(x =>
            {
                var parameters = x.GetParameters();
                return x.IsPublic && parameters.Length == 1 && parameters[0].ParameterType == type;
            });
        }


        public static object? ConvertFromString(Type type, string value)
        {
            // 1. Try get type descriptor.

            if (TypeDescriptor.GetConverter(type) is { } converter)
            {
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFromInvariantString(value);
                }
            }

            // 2. Find proper constructor.
            var constructor = type.TryGetConstructor(typeof(string));

            if (constructor != null)
            {
                var instance = constructor.Invoke(new[] { value });
                return instance;
            }

            return null;
        }
    }
}

namespace Neobyte.Build.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CommandLineOptionAttribute : Attribute
    {
        public readonly string Name;
        public readonly string? Description;

        public CommandLineOptionAttribute(string name)
        {
            this.Name = name;
        }

        public CommandLineOptionAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}

namespace Neobyte.Build.Core
{
    public class CommandLine
    {
        private readonly Dictionary<string, string?> m_Values = new();

        public CommandLine(string[] args)
        {
            foreach (var arg in args)
            {
                var view = arg.AsSpan();

                var separator = view.IndexOfAny(':', '=');

                if (separator >= 0)
                {
                    var name = view.Slice(start: 0, length: separator);
                    var value = view[(separator + 1)..];

                    this.m_Values.Add(name.ToString(), value.ToString());
                }
                else
                {
                    this.m_Values.Add(arg, null);
                }
            }
        }

        public void Apply(object instance)
        {
            this.Apply(instance.GetType(), instance, isStatic: false);
        }

        public void Apply(Type type)
        {
            this.Apply(type, null, isStatic: true);
        }

        public void Apply<T>()
        {
            this.Apply(typeof(T));
        }

        private void Apply(Type type, object? instance, bool isStatic)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (method.IsStatic == isStatic)
                {
                    var option = method.GetCustomAttribute<CommandLineOptionAttribute>();

                    if (option != null)
                    {
                        if (this.m_Values.TryGetValue(option.Name, out var argValue))
                        {
                            var parameters = method.GetParameters();

                            if (parameters.Length == 0)
                            {
                                if (argValue == null)
                                {
                                    method.Invoke(instance, null);
                                }
                                else
                                {
                                    throw new NotSupportedException($@"Command line option {option.Name} requires no arguments");
                                }
                            }
                            else if (parameters.Length == 1)
                            {
                                var parameterType = parameters[0].ParameterType;
                                var underlying = Nullable.GetUnderlyingType(parameterType) ?? parameterType;

                                if (argValue != null)
                                {
                                    var value = CommandLineValueConverter.ConvertFromString(underlying, argValue);

                                    method.Invoke(instance, new[] { value });
                                }
                                else
                                {
                                    throw new NotSupportedException($@"Command line option {option.Name} requires exactly one argument of type {underlying}");
                                }
                            }
                            else
                            {
                                throw new NotSupportedException($@"Command line option {option.Name} has more than one option");
                            }
                        }
                    }
                }
            }

#if false
            var fields = type.GetFields();

            foreach (var field in fields)
            {
                if (field.IsStatic == isStatic)
                {
                    var option = field.GetCustomAttribute<CommandLineOptionAttribute>();

                    if (option != null)
                    {
                        if (this.m_Values.TryGetValue(option.Name, out var argValue))
                        {
                            if (argValue != null)
                            {
                                var value = CommandLineValueConverter.ConvertFromString(field.FieldType, argValue);
                                field.SetValue(instance, value);
                            }
                            else
                            {
                                throw new NotSupportedException($@"Command line option {option.Name} is not supported");
                            }
                        }
                    }
                }
            }

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var setter = property.SetMethod;
                if (setter != null)
                {
                    if (setter.Attributes.HasFlag(MethodAttributes.Static) == isStatic)
                    {
                        var option = property.GetCustomAttribute<CommandLineOptionAttribute>();

                        if (option != null)
                        {
                            if (this.m_Values.TryGetValue(option.Name, out var argValue))
                            {
                                if (argValue != null)
                                {
                                    var value = CommandLineValueConverter.ConvertFromString(property.PropertyType, argValue);
                                    setter.Invoke(instance, new[] { value });
                                }
                                else
                                {
                                    throw new NotSupportedException($@"Command line option {option.Name} is not supported");
                                }
                            }
                        }
                    }
                }
            }
#endif
        }
    }
}
