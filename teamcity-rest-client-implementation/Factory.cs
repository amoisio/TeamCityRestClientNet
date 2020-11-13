using System;
using System.Text;

namespace TeamCityRestClientNet
{
    /**
  * Factory object to create new object of [TeamCityInstance] interface
  *
  * @see TeamCityInstance
  */
    public static class TeamCityInstanceFactory
    {
        /**
        * Creates guest authenticated accessor
        * @param serverUrl HTTP or HTTPS URL to TeamCity server
        *
        * Used via reflection for backward compatibility for deprecated methods
        */
        public static TeamCityInstance guestAuth(string serverUrl)
          => createGuestAuthInstance(serverUrl);

        internal static TeamCityInstanceImpl createGuestAuthInstance(string serverUrl)
          => new TeamCityInstanceImpl(serverUrl.TrimEnd('/'), "/guestAuth", null, false);

        /**
        * Creates username/password authenticated accessor
        *
        * @param serverUrl HTTP or HTTPS URL to TeamCity server
        * @param username username
        * @param password password
        *
        * Used via reflection for backward compatibility for deprecated methods
        */
        public static TeamCityInstance httpAuth(string serverUrl, string username, string password)
          => createHttpAuthInstance(serverUrl, username, password);

        internal static TeamCityInstanceImpl createHttpAuthInstance(string serverUrl, string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var authorization = Convert.ToBase64String(bytes);
            return new TeamCityInstanceImpl(serverUrl.TrimEnd('/'), "/httpAuth", "Basic $authorization", false);
        }

        /**
        * Creates token based connection.
        * TeamCity access token generated on My Settings & Tools | Access Tokens
        *
        * @param serverUrl HTTP or HTTPS URL to TeamCity server
        * @param token token
        *
        * see https://www.jetbrains.com/help/teamcity/rest-api.html#RESTAPI-RESTAuthentication
        */
        public static TeamCityInstance tokenAuth(string serverUrl, string token)
          => createTokenAuthInstance(serverUrl, token);

        internal static TeamCityInstanceImpl createTokenAuthInstance(string serverUrl, string token)
          => new TeamCityInstanceImpl(serverUrl.TrimEnd('/'), "", "Bearer $token", false);
    }
}