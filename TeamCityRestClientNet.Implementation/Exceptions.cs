using System;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet
{

    public class TeamCityQueryException : TeamCityException
    {
        public TeamCityQueryException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }

    public class TeamCityConversationException : TeamCityException
    {
        public TeamCityConversationException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }
}