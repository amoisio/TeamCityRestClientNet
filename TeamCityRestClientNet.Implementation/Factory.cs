using System;
using System.Text;
using BAMCIS.Util.Concurrent;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Domain
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
        public static TeamCity GuestAuth(string serverUrl)
          => CreateGuestAuthInstance(serverUrl);

        internal static TeamCityInstance CreateGuestAuthInstance(string serverUrl)
          => new TeamCityInstance(serverUrl.TrimEnd('/'), "/guestAuth", null, false, TimeUnit.MINUTES);

        /**
        * Creates username/password authenticated accessor
        *
        * @param serverUrl HTTP or HTTPS URL to TeamCity server
        * @param username username
        * @param password password
        *
        * Used via reflection for backward compatibility for deprecated methods
        */
        public static TeamCity HttpAuth(string serverUrl, string username, string password)
          => CreateHttpAuthInstance(serverUrl, username, password);

        internal static TeamCityInstance CreateHttpAuthInstance(string serverUrl, string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var authorization = Convert.ToBase64String(bytes);
            return new TeamCityInstance(serverUrl.TrimEnd('/'), "/httpAuth", $"Basic {authorization}", false, TimeUnit.MINUTES);
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
        public static TeamCity TokenAuth(string serverUrl, string token)
          => CreateTokenAuthInstance(serverUrl, token);

        internal static TeamCityInstance CreateTokenAuthInstance(string serverUrl, string token)
          => new TeamCityInstance(serverUrl.TrimEnd('/'), "", $"Bearer {token}", false, TimeUnit.MINUTES);
    }
}