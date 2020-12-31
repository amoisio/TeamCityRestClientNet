using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TeamCityRestClientNet.Api
{
    public static class Utilities
    {
        public static DateTimeOffset? ParseTeamCity(string teamCityDateTime)
        {
            if (String.IsNullOrEmpty(teamCityDateTime))
                return null;
            else
                return DateTimeOffset.ParseExact(
                    teamCityDateTime,
                    Constants.TEAMCITY_DATETIME_FORMAT,
                    CultureInfo.CurrentCulture);
        }
    }
}