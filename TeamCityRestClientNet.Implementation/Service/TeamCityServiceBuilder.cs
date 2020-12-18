using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private long _timeout;
        private ILogger _logger;
        private RefitSettings _settings;
        private Func<HttpMessageHandler, HttpMessageHandler>[] _handlers;
        private bool _withDefaultHandlers;

        public TeamCityServiceBuilder(ILogger logger)
        {
            this._logger = logger ?? NullLogger.Instance;
        }

        public string ServerUrl => _serverUrl;
        public string ServerUrlBase => _serverUrlBase ?? "";

        public TeamCityServiceBuilder WithServerUrl(string serverUrl, string serverUrlBase)
        {
            _serverUrl = serverUrl;
            _serverUrlBase = serverUrlBase;
            return this;
        }

        /// <summary>
        /// Set query timeout.
        /// </summary>
        /// <param name="timeout">Query timeout in seconds.</param>
        public TeamCityServiceBuilder WithTimeout(long timeout)
        {
            _timeout = timeout;
            return this;
        }

        /// <summary>
        /// Set the token store used for Bearer authentication.
        /// </summary>
        /// <param name="tokenStore">Token store.</param>
        public TeamCityServiceBuilder WithBearerTokenStore(IBearerTokenStore tokenStore)
        {
            _bearerTokenStore = tokenStore;
            return this;
        }

        /// <summary>
        /// Set the token store used for CSRF tokens.
        /// </summary>
        /// <param name="csrfStore">Token store.</param>
        public TeamCityServiceBuilder WithCSRFTokenStore(ICSRFTokenStore csrfStore)
        {
            _csrfTokenStore = csrfStore;
            return this;
        }

        public TeamCityServiceBuilder WithRefitSettings(RefitSettings settings)
        {
            _settings = settings;
            return this;
        }

        public TeamCityServiceBuilder WithDefaultHandlers()
        {
            _withDefaultHandlers = true;
            return this;
        }

        public TeamCityServiceBuilder WithHandlers(params Func<HttpMessageHandler, HttpMessageHandler>[] handlers)
        {
            _handlers = handlers;
            return this;
        }

        internal ITeamCityService Build()
        {
            ValidateProperties();

            var hostUrl = $"{_serverUrl}{ServerUrlBase}";

            _logger.LogInformation($"Building REST service to {hostUrl}.");

            var serviceHandler = BuildHandlerPipeline();
            var settings = BuildRefitSettings();
            
            return RestService.For<ITeamCityService>(
                new HttpClient(serviceHandler) 
                { 
                    BaseAddress = new Uri(hostUrl) 
                },
                settings);
        }

        private void ValidateProperties()
        {
            if (String.IsNullOrWhiteSpace(_serverUrl))
                throw new ArgumentException("ServerUrl must be provided.");

            if (_bearerTokenStore == null)
                throw new ArgumentException("BearerTokenStore must be provided.");
        }

        private HttpMessageHandler BuildHandlerPipeline()
        {
            var handlers = new List<Func<HttpMessageHandler, HttpMessageHandler>>();
            if (_withDefaultHandlers) 
            {
                handlers.Add((innerHandler) => new BearerTokenHandler(_bearerTokenStore, innerHandler));
                // CSRFTokenHandler csrfHandler = null;
                // TODO: Enable when using csrf
                // if (_csrfTokenStore != null)
                //     csrfHandler = new CSRFTokenHandler(_csrfTokenStore);
                handlers.Add((innerHandler) => new LoggingHandler(_logger));
            }

            if (_handlers != null)
            {
                handlers.AddRange(_handlers);
            }

            HttpMessageHandler innerHandler = null;
            int count = _handlers.Length;
            for(int i = count - 1; i >= 0; i--)
            {
                innerHandler = _handlers[i](innerHandler);
            }
            
            return innerHandler;
        }

        private RefitSettings BuildRefitSettings() 
        {
            if (_settings == null)
            {
                _settings = new RefitSettings();
                _settings.ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
            return _settings;
        } 
    }
}