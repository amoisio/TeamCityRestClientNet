using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Authentication
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly LogOptions _options;

        public LoggingHandler(ILogger logger, LogOptions options, HttpMessageHandler innerHandler = null)
           : base(innerHandler ?? new HttpClientHandler())
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? new LogOptions();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_options.LogRequest)
                await LogRequest(request).ConfigureAwait(false);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (_options.LogResponse)
                await LogResponse(response).ConfigureAwait(false);

            return response;
        }

        private async Task LogRequest(HttpRequestMessage request)
        {
            var sb = new StringBuilder();
            sb.Append($"HTTP/{request.Version} ");
            sb.Append($"{request.Method.Method} ");
            sb.Append($"{request.RequestUri} - ");
            if (_options.LogRequestHeaders)
            {
                foreach (var header in request.Headers)
                {
                    sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
                }
            }
            sb.AppendLine();
            _logger.LogTrace(sb.ToString());

            if (_options.LogRequestContent)
            {
                sb.Clear();
                if (request.Content != null)
                {
                    sb.Append($"Content ");
                    foreach (var header in request.Content.Headers)
                    {
                        sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
                    }
                    var body = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    sb.AppendLine($"{body}");
                }
                _logger.LogTrace(sb.ToString());
            }
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            var sb = new StringBuilder();
            sb.Append($"HTTP/{response.Version} ");
            sb.Append($"{response.StatusCode} ");
            sb.Append($"{response.ReasonPhrase} ");
            if (_options.LogResponseHeaders)
            {
                foreach (var header in response.Headers)
                {
                    sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
                }
            }
            sb.AppendLine();
            _logger.LogTrace(sb.ToString());

            if (_options.LogResponseContent)
            {
                sb.Clear();
                if (response.Content != null)
                {
                    sb.Append($"Content ");
                    foreach (var header in response.Content.Headers)
                    {
                        sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
                    }
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    sb.AppendLine($"{body}");
                }
                _logger.LogTrace(sb.ToString());
            }
        }
    }
}