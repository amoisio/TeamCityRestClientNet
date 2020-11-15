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

        public static string Let<T>(this T value, string pattern)
            => (value == null) ? null : string.Format(pattern, value);

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