using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Tools
{
    public static class Utility
    {
        public static List<T> ListOfNotNull<T>(params T[] values)
            => values.Where(val => val != null).ToList();

        public const int REASONABLE_MAX_PAGE_SIZE = 1024;

        public static int? SelectRestApiCountForPagedRequests(int? limitResults, int? pageSize)
        {
            if (!limitResults.HasValue && !pageSize.HasValue)
                return null;

            int tmp;
            if (limitResults.HasValue)
            {
                tmp = limitResults.Value;
                if (pageSize.HasValue)
                    tmp = Math.Min(tmp, pageSize.Value);
            }
            else 
                tmp = pageSize.Value;

            return Math.Min(tmp, REASONABLE_MAX_PAGE_SIZE);
        }


        // private fun SaveToFile(response: retrofit.client.Response, file: File) {
        //     file.parentFile.mkdirs()
        //     val input = response.body.`in`()
        //     BufferedOutputStream(FileOutputStream(file)).use {
        //         input.copyTo(it)
        //     }
        // }
    }


}