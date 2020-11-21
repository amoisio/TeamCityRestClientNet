using System;
using System.Collections.Generic;

namespace TeamCityRestClientNet.Extensions
{
    public static class Extensions
    {
        public static bool IsNotEmpty<T>(this IList<T> list)
            => list.Count > 0;

        public static bool IsEmpty<T>(this IList<T> list)
            => list.Count == 0;


        /// <summary>
        /// Returns the string value iff string is non-empty, otherwise null.
        /// </summary>
        public static string ValueOrNull(this string str)
         => String.IsNullOrWhiteSpace(str) ? null : str;

        public static string SubstringAfter(this string str, string pattern, string missingDelimiter = null)
        {
            if (String.IsNullOrWhiteSpace(str))
                return str;
            
            int index = str.IndexOf(pattern);
            if (index == -1)
                return missingDelimiter == null ? str : missingDelimiter;

            return str.Substring(index + 1);
        }

        public static string SubstringBefore(this string str, string pattern, string missingDelimiter = null)
        {
            if (String.IsNullOrWhiteSpace(str))
                return str;

            int index = str.IndexOf(pattern);
            if (index == -1)
                return missingDelimiter == null ? str : missingDelimiter;

            return str.Substring(0, index);
        }

        /// <summary>
        /// Runs a transformation on the item and returns its result. 
        /// If item is null, short-circuits and returns null.
        /// </summary>
        public static U Let<T, U>(this T item, Func<T, U> transform)
            => (item == null) ? default(U) : transform(item);

        public static string Let<T>(this T value, string pattern)
            => (value == null) ? null : string.Format(pattern, value);

        /// <summary>
        /// Throws nullRef if value is null or (empty string). Otherwise, 
        /// returns the value.
        /// </summary>
        public static T SelfOrNullRef<T>(this T value)
            => (value == null || value is string s && String.IsNullOrEmpty(s))
                ? throw new NullReferenceException()
                : value;
        public static string RemovePrefix(this string str, string prefix)
        {
            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(prefix))
            {
                return str;
            }
            else
            {
                var index = str.IndexOf(prefix, StringComparison.CurrentCultureIgnoreCase);
                if (index == 0)
                {
                    return str.Substring(0, prefix.Length);
                }
                else
                {
                    return str;
                }
            }
        }
    }
}