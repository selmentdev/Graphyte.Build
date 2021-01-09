using System;
using System.Runtime.CompilerServices;

namespace Neobyte.Build.Core
{
    public static class ArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToCharUpper(int value)
        {
            value &= 0xF;
            value += 48;

            if (value > 57)
            {
                value += 7;
            }

            return (char)value;
        }

        public static string ToString(byte[] value)
        {
            return ToString(value, 0, value.Length);
        }

        public static string ToString(byte[] value, int startIndex)
        {
            return ToString(value, startIndex, value.Length - startIndex);
        }

        public static string ToString(byte[] value, int startIndex, int length)
        {
            return string.Create(length * 2, (value, startIndex, length), delegate (Span<char> outView, (byte[] value, int startIndex, int length) state)
            {
                var inView = new ReadOnlySpan<byte>(state.value, state.startIndex, state.length);

                var inIndex = 0;
                var outIndex = 0;

                while (inIndex < inView.Length)
                {
                    var current = inView[inIndex++];
                    outView[outIndex++] = ToCharUpper(current >> 4);
                    outView[outIndex++] = ToCharUpper(current);
                }
            });
        }

    }
}
