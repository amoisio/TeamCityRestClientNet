using System;
using System.Net.Http;
using BAMCIS.Util.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Refit;
using TeamCityRestClientNet.Authentication;

namespace TeamCityRestClientNet.Service
{
    public class TeamCityServiceBuilder
    {
        private string _serverUrl;
        private string _serverUrlBase;
        private IAuthTokenStore _tokenStore;
        private TimeUnit _unit;
        private long _timeout;
        private ILogger _logger;

        public TeamCityServiceBuilder(ILogger logger)
        {
            this._logger = logger ?? NullLogger.Instance;
        }

        public string ServerUrl => _serverUrl;
        public string ServerUrlBase => _serverUrlBase ?? "";
        public TeamCityServiceBuilder SetServerUrl(string serverUrl, string serverUrlBase)
        {
            _serverUrl = serverUrl;
            _serverUrlBase = serverUrlBase;
            return this;
        }

        public TeamCityServiceBuilder SetTimeout(TimeUnit unit, long timeout)
        {
            _unit = unit;
            _timeout = timeout;
            return this;
        }

        public TeamCityServiceBuilder SetTokenStore(IAuthTokenStore tokenStore)
        {
            _tokenStore = tokenStore;
            return this;
        }

        internal ITeamCityService Build()
        {
            ValidateProperties();

            var hostUrl = $"{_serverUrl}{ServerUrlBase}";
            var settings = new RefitSettings { };

            _logger.LogInformation($"Building REST service to {hostUrl}.");
            return RestService.For<ITeamCityService>(
                new HttpClient(new TokenHeaderAuthenticationHandler(_tokenStore))
                {
                    BaseAddress = new Uri(hostUrl)
                },
                settings);
        }

        private void ValidateProperties()
        {
            if (String.IsNullOrWhiteSpace(_serverUrl))
                throw new ArgumentException("ServerUrl must be provided.");

            if (_tokenStore == null)
                throw new ArgumentException("TokenStore must be provided.");
        }
    }
}