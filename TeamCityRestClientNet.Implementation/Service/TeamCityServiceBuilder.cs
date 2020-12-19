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
        private LogOptions _options;
        private RefitSettings _settings;
        private Func<HttpMessageHandler, HttpMessageHandler>[] _handlers;
        private bool _omitDefaultHandlers;

        public TeamCityServiceBuilder()
        {
            this._logger = NullLogger.Instance;

        }

        /// <summary>
        /// TeamCity server (host) url. Eg. If TC REST API is at http://localhost:5000/custom/app/rest/
        /// then ServerUrl should be "http://localhost:5000".
        /// </summary>
        public string ServerUrl => _serverUrl;

        /// <summary>
        /// TeamCity REST API url base. E.g if TC REST API is at http://localhost:5000/custom/app/rest/
        /// then ServerUrlBase should be "/custom".
        /// </summary>
        public string ServerUrlBase => _serverUrlBase ?? "";

        /// <summary>
        /// Configure the url of the TeamCity REST API.
        /// </summary>
        /// <param name="serverUrl">TeamCity server (host) url.</param>
        /// <param name="serverUrlBase">TeamCity REST API url base address.</param>
        public TeamCityServiceBuilder WithServerUrl(string serverUrl, string serverUrlBase = "")
        {
            _serverUrl = serverUrl.TrimEnd('/');
            _serverUrlBase = serverUrlBase?.TrimEnd('/');
            return this;
        }

        /// <summary>
        /// Configure the query timeout used with the TeamCity REST API.
        /// </summary>
        /// <param name="timeout">Query timeout in seconds.</param>
        public TeamCityServiceBuilder WithTimeout(long timeout)
        {
            _timeout = timeout;
            return this;
        }

        /// <summary>
        /// Configure the store used for retrieving the bearer token.
        /// </summary>
        /// <param name="tokenStore">Token store.</param>
        public TeamCityServiceBuilder WithBearerTokenStore(IBearerTokenStore tokenStore)
        {
            _bearerTokenStore = tokenStore;
            return this;
        }

        /// <summary>
        /// Configure the store used for retrieving CSRF tokens.
        /// </summary>
        /// <param name="csrfStore">Token store.</param>
        public TeamCityServiceBuilder WithCSRFTokenStore(ICSRFTokenStore csrfStore)
        {
            _csrfTokenStore = csrfStore;
            return this;
        }

        /// <summary>
        /// Configure Refit settings. See <see cref="https://github.com/reactiveui/refit">.
        /// </summary>
        /// <param name="settings">Refit settings.</param>
        public TeamCityServiceBuilder WithRefitSettings(RefitSettings settings)
        {
            _settings = settings;
            return this;
        }

        /// <summary>
        /// Configure the TeamCity client to not use default HTTP message handlers.
        /// Default handlers do the following:
        /// 1. Set the Bearer authentication token in every request.
        /// 2. Set the CSRF token in POST, PUT and DELETE requests.
        /// 3. Log every request and response with Trace level.
        /// </summary>
        public TeamCityServiceBuilder OmitDefaultHandlers()
        {
            _omitDefaultHandlers = true;
            return this;
        }

        /// <summary>
        /// Configure the HTTP message handler used when processing TeamCity queries.
        /// The provided handlers will be added to the HTTP message pipeline so that
        /// they are executed after Bearer and CSRF token have been added but before 
        /// request and response logging is done.
        /// </summary>
        /// <param name="handlers">Handlers to use.</param>
        public TeamCityServiceBuilder WithHandlers(params Func<HttpMessageHandler, HttpMessageHandler>[] handlers)
        {
            _handlers = handlers;
            return this;
        }

        /// <summary>
        /// Configure logging for TeamCity queries.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="options">Logging options.</param>
        public TeamCityServiceBuilder WithLogging(ILogger logger, LogOptions options = null)
        {
            _logger = logger;
            _options = options ?? new LogOptions();
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
                throw new InvalidOperationException("Server url must be provided.");

            if (!_omitDefaultHandlers)
            {
                if (_bearerTokenStore == null)
                    throw new InvalidOperationException("Bearer token store must be provided.");

                if (_csrfTokenStore == null)
                    throw new InvalidOperationException("CSRF token store must be provided.");
            }
        }

        private HttpMessageHandler BuildHandlerPipeline()
        {
            var handlers = new List<Func<HttpMessageHandler, HttpMessageHandler>>();
            if (!_omitDefaultHandlers)
            {
                handlers.Add((innerHandler) => new BearerTokenHandler(_bearerTokenStore, innerHandler));
                // CSRFTokenHandler csrfHandler = null;
                // TODO: Enable when using csrf
                // if (_csrfTokenStore != null)
                //     csrfHandler = new CSRFTokenHandler(_csrfTokenStore);
            }

            if (_handlers != null)
            {
                handlers.AddRange(_handlers);
            }

            if (!_omitDefaultHandlers)
            {
                handlers.Add((innerHandler) => new LoggingHandler(_logger, _options));
            }

            HttpMessageHandler innerHandler = null;
            int count = handlers.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                innerHandler = handlers[i](innerHandler);
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