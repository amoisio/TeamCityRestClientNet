using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Authentication;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet
{
    public class TeamCityServerBuilder
    {
        private string _serverUrl;
        private string _serverUrlBase;
        private string _bearerToken;
        private ICSRFTokenStore _csrfTokenStore;
        private long _timeout;
        private ILogger _logger;
        private LogOptions _options;
        private RefitSettings _settings;
        private Func<HttpMessageHandler, HttpMessageHandler>[] _handlers;

        /**
        * Creates token based connection.
        * TeamCity access token generated on My Settings & Tools | Access Tokens
        *
        * @param serverUrl HTTP or HTTPS URL to TeamCity server
        * @param token token
        *
        * see https://www.jetbrains.com/help/teamcity/rest-api.html#RESTAPI-RESTAuthentication
        */
        public static TeamCity CreateTokenAuthInstance(string serverUrl, string token, ILogger logger)
        {
            return new TeamCityServerBuilder()
              .WithServerUrl(serverUrl)
              .WithBearerAuthentication(token)
              .WithLogging(logger)
              .Build();
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
        public TeamCityServerBuilder WithServerUrl(string serverUrl, string serverUrlBase = "")
        {
            _serverUrl = serverUrl.TrimEnd('/');
            _serverUrlBase = serverUrlBase?.TrimEnd('/');
            return this;
        }

        /// <summary>
        /// Configure the query timeout used with the TeamCity REST API.
        /// </summary>
        /// <param name="timeout">Query timeout in seconds.</param>
        public TeamCityServerBuilder WithQueryTimeout(long timeout)
        {
            _timeout = timeout;
            return this;
        }

        /// <summary>
        /// Configure the token used for Bearer token authentication.
        /// </summary>
        /// <param name="bearerToken">Bearer token.</param>
        public TeamCityServerBuilder WithBearerAuthentication(string bearerToken)
        {
            _bearerToken = bearerToken;
            return this;
        }

        /// <summary>
        /// Configure the store used for retrieving CSRF tokens for state altering HTTP calls such as
        /// POST, PUT and DELETE.
        /// </summary>
        /// <param name="csrfStore">Token store.</param>
        public TeamCityServerBuilder WithCSRF(ICSRFTokenStore csrfStore)
        {
            _csrfTokenStore = csrfStore;
            return this;
        }

        /// <summary>
        /// Configure Refit settings. See <see cref="https://github.com/reactiveui/refit">.
        /// </summary>
        /// <param name="settings">Refit settings.</param>
        public TeamCityServerBuilder WithRefitSettings(RefitSettings settings)
        {
            _settings = settings;
            return this;
        }

        /// <summary>
        /// Configure the HTTP message handler used when processing TeamCity queries.
        /// The provided handlers will be added to the HTTP message pipeline so that
        /// they are executed after Bearer and CSRF token have been added but before 
        /// request and response logging is done.
        /// </summary>
        /// <param name="handlers">Handlers to use.</param>
        public TeamCityServerBuilder WithHandlers(params Func<HttpMessageHandler, HttpMessageHandler>[] handlers)
        {
            _handlers = handlers;
            return this;
        }

        /// <summary>
        /// Configure logging for TeamCity queries.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="options">Logging options.</param>
        public TeamCityServerBuilder WithLogging(ILogger logger, LogOptions options = null)
        {
            _logger = logger;
            _options = options ?? new LogOptions();
            return this;
        }

        public virtual TeamCity Build()
        {
            ValidateProperties();

            var hostUrl = $"{_serverUrl}{ServerUrlBase}";

            _logger.LogInformation($"Building REST service to {hostUrl}.");

            var serviceHandler = BuildHandlerPipeline();
            var settings = BuildRefitSettings();
            var service = RestService.For<ITeamCityService>(
                new HttpClient(serviceHandler)
                {
                    BaseAddress = new Uri(hostUrl)
                },
                settings);

            return new TeamCityServer(_serverUrl, _serverUrlBase, service, _logger);
        }

        private void ValidateProperties()
        {
            if (String.IsNullOrWhiteSpace(_serverUrl))
                throw new InvalidOperationException("Server url must be provided.");
        }

        private HttpMessageHandler BuildHandlerPipeline()
        {
            var handlers = new List<Func<HttpMessageHandler, HttpMessageHandler>>();

            if (!String.IsNullOrWhiteSpace(_bearerToken))
                handlers.Add((innerHandler) => new BearerAuthenticationHandler(_serverUrl, _bearerToken, innerHandler));

            if (_csrfTokenStore != null)
                handlers.Add((innerHandler) => new CSRFTokenHandler(_csrfTokenStore, innerHandler));

            if (_handlers != null)
                handlers.AddRange(_handlers);

            if (_logger != null)
                handlers.Add((innerHandler) => new LoggingHandler(_logger, _options));

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