using System;

namespace TeamCityRestClientNet
{
    public class TeamCityException : Exception
    {
        public TeamCityException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }
}