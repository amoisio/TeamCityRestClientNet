using System;
using System.Net.Http;
using BAMCIS.Util.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using TeamCityRestClientNet.Authentication;

namespace TeamCityRestClientNet.Service
{
    public class TeamCityServiceBuilder
    {
        private string _serverUrl;
        private string _serverUrlBase;
        private IBearerTokenStore _bearerTokenStore;
        private ICSRFTokenStore _csrfTokenStore;
        private TimeUnit _unit;
        private long _timeout;
        private ILogger _logger;
        private RefitSettings _settings;

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

        public TeamCityServiceBuilder SetBearerTokenStore(IBearerTokenStore tokenStore)
        {
            _bearerTokenStore = tokenStore;
            return this;
        }

        public TeamCityServiceBuilder SetCSRFTokenStore(ICSRFTokenStore csrfStore)
        {
            _csrfTokenStore = csrfStore;
            return this;
        }

        public TeamCityServiceBuilder SetRefitSettings(RefitSettings settings)
        {
            _settings = settings;
            return this;
        }

        internal ITeamCityService Build()
        {
            ValidateProperties();

            var hostUrl = $"{_serverUrl}{ServerUrlBase}";

            _logger.LogInformation($"Building REST service to {hostUrl}.");

            CSRFTokenHandler csrfHandler = null;
            // TODO: Enable when using csrf
            // if (_csrfTokenStore != null)
            //     csrfHandler = new CSRFTokenHandler(_csrfTokenStore);

            if (_settings == null)
            {
                _settings = new RefitSettings();
                _settings.ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }

            var serviceHandler = new BearerTokenHandler(_bearerTokenStore, csrfHandler);
            return RestService.For<ITeamCityService>(
                    new HttpClient(serviceHandler) { BaseAddress = new Uri(hostUrl) }, _settings);
        }

        private void ValidateProperties()
        {
            if (String.IsNullOrWhiteSpace(_serverUrl))
                throw new ArgumentException("ServerUrl must be provided.");

            if (_bearerTokenStore == null)
                throw new ArgumentException("BearerTokenStore must be provided.");
        }
    }
}