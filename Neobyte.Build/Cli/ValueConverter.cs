using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Neobyte.Build.Cli
{
    static class ValueConverter
    {
        private static bool IsEnumerable(this Type self)
        {
            if (self == typeof(string))
            {
                return false;
            }

            if (self.IsArray)
            {
                return true;
            }

            return self.IsGenericType && self.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private static Type? GetElementTypeIfEnumerable(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            var enumerable = type.IsEnumerable()
                ? type
                : type.GetInterfaces().FirstOrDefault(IsEnumerable);

            return enumerable?.GenericTypeArguments[0];
        }

        private static ConstructorInfo? TryGetConstructor(this Type self, Type type)
        {
            return self.GetConstructors().SingleOrDefault(x =>
            {
                var parameters = x.GetParameters();
                return x.IsPublic && parameters.Length == 1 && parameters[0].ParameterType == type;
            });
        }

        public static object? FromString(Type type, string value)
        {
            if (TypeDescriptor.GetConverter(type) is { } converter)
            {
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFromInvariantString(value);
                }
            }

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
