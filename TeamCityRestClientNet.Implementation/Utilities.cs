using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Tools
{
    public static class Utilities
    {
        public static List<T> ListOfNotNull<T>(params T[] values)
            => values.Where(val => val != null).ToList();


        public static DateTimeOffset? ParseTeamCity(string teamCityDateTime)
        {
            if (String.IsNullOrEmpty(teamCityDateTime))
                return null;
            else
                return DateTimeOffset.ParseExact(
                    teamCityDateTime,
                    TeamCityInstance.TEAMCITY_DATETIME_FORMAT,
                    CultureInfo.CurrentCulture);
        }

        public static int? SelectRestApiCountForPagedRequests(int? limitResults, int? pageSize)
        {
            var reasonableMaxPageSize = 1024;
            return pageSize ?? limitResults?.Let((val) => Math.Min(val, reasonableMaxPageSize));
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