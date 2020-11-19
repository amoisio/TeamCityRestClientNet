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

        public static string SubstringAfter(this string str, string pattern)
        {
            if (String.IsNullOrWhiteSpace(str))
                return str;
            
            int index = str.IndexOf(pattern);
            if (index == -1)
                return str;

            return str.Substring(index + 1);
        }

        /// <summary>
        /// Runs a transformation on the item and returns its result. 
        /// If item is null, short-circuits and returns null.
        /// </summary>
        public static U Let<T, U>(this T item, Func<T, U> transform)
            => (item == null) ? default(U) : transform(item);

        public static string Let<T>(this T value, string pattern)
            => (value == null) ? null : string.Format(pattern, value);

        public static T SelfOrNullRefException<T>(this T value)
            => (value is string s && String.IsNullOrEmpty(s) || value == null)
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