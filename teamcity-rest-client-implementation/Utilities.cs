using System.Collections.Generic;
using System.Linq;

namespace TeamCityRestClientNet.Utilities
{
    public static class ListUtilities
    {
        public static List<T> ListOfNotNull<T>(params T[] values)
            => values.Where(val => val != null).ToList();
    }
}