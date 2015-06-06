using System;
using System.Runtime.CompilerServices;

namespace h0wXD.Helpers
{
    public static class StringHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Prepend(this string text, string value, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            if (!text.StartsWith(value, _comparisonType))
            {
                text = value + text;
            }

            return text;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Append(this string text, string value, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            if (!text.EndsWith(value, _comparisonType))
            {
                text = text + value;
            }

            return text;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Wrap(this string text, string value, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            return Prepend(Append(text, value, _comparisonType), value, _comparisonType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Wrap(this string text, string valueBefore, string valueAfter, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            return Prepend(Append(text, valueAfter, _comparisonType), valueBefore, _comparisonType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Strip(this string text, string value, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            if (text.StartsWith(value, _comparisonType))
            {
                text = text.Substring(value.Length);
            }

            if (text.EndsWith(value, _comparisonType))
            {
                text = text.Substring(0, text.Length - value.Length);
            }

            return text;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWith(this string text, string value, int minimumMatchCount, StringComparison _comparisonType = StringComparison.CurrentCulture)
        {
            minimumMatchCount = Math.Min(text.Length, minimumMatchCount);

            var textToMatch = value.Substring(0, Math.Min(value.Length, minimumMatchCount));

            if (value.Length == text.Length)
            {
                return value.Equals(text, _comparisonType);
            }

            return text.StartsWith(textToMatch, _comparisonType);
        }
    }
}
