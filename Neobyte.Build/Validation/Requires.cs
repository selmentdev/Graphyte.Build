using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Threading.Tasks;

namespace Neobyte.Build.Validation
{
    public static class Requires
    {
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static T NotNull<T>(T value, string name)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNull(Task value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNull<T>(Task<T> value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static T NotNullBoxed<T>(T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNullOrEmpty(string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (value.Length == 0 || value[0] == '\0')
            {
                throw new ArgumentException($@"Parameter {name} is empty string");
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNullOrWhiteSpace(string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (value.Length == 0 || value[0] == '\0')
            {
                throw new ArgumentException($@"Parameter {name} is empty string");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($@"Parameter {name} contains only whitespace characters");
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNullOrEmpty(IEnumerable values, string name)
        {
            if (values == null)
            {
                throw new ArgumentNullException(name);
            }

            var empty = true;

            var enumerator = values.GetEnumerator();

            try
            {
                if (enumerator.MoveNext())
                {
                    empty = false;
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            if (empty)
            {
                throw new ArgumentException($@"Enumerable {name} is empty");
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNullEmptyOrNullElements<T>(IEnumerable<T> values, string name)
            where T: class
        {
            NotNull(values, name);

            var empty = true;

            foreach (var value in values)
            {
                empty = false;

                if (value == null)
                {
                    throw new ArgumentException($@"Sequence {name} contains null element");
                }
            }

            if (empty)
            {
                throw new ArgumentException($@"Enumerable {name} is empty");
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NullOrNotNullElements<T>(IEnumerable<T> values, string name)
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    if(value == null)
                    {
                        throw new ArgumentException($@"Sequence {name} contains null element");
                    }
                }
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotEmpty(Guid value, string name)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($@"{name} cannot be empty guid");
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void Range(bool condition, string name, string message = null)
        {
            if (!condition)
            {
                FailRange(name, message);
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Exception FailRange(string name, string message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentOutOfRangeException(name);
            }

            throw new ArgumentOutOfRangeException(name, message);
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void Argument(bool condition, string name, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message, name);
            }
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void Fail(string message)
        {
            throw new ArgumentException(message);
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void Fail(Exception innerException, string message)
        {
            throw new ArgumentException(message, innerException);
        }
    }
}
