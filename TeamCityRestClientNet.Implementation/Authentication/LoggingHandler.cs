using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TeamCityRestClientNet.Authentication
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public LoggingHandler(ILogger logger, HttpMessageHandler innerHandler = null)
           : base(innerHandler ?? new HttpClientHandler())
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await LogRequest(request).ConfigureAwait(false);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            await LogResponse(response).ConfigureAwait(false);
            return response;
        }


        private async Task LogRequest(HttpRequestMessage request)
        {
            var sb = new StringBuilder();
            sb.Append($"HTTP/{request.Version.ToString()} ");
            sb.Append($"{request.Method.Method} ");
            sb.Append($"{request.RequestUri} - ");
            foreach (var header in request.Headers)
            {
                sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
            }
            sb.AppendLine($"END.");
            if (request.Content != null)
            {
                sb.Append($"Content ");
                foreach (var header in request.Content.Headers)
                {
                    sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
                }
                var body = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                sb.AppendLine($"{body} END.");
            }
            _logger.LogTrace(sb.ToString());
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            var sb = new StringBuilder();
            sb.Append($"HTTP/{response.Version.ToString()} ");
            sb.Append($"{response.StatusCode} ");
            sb.Append($"{response.ReasonPhrase} ");
            foreach (var header in response.Headers)
            {
                sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
            }
            sb.AppendLine($"END.");
            if (response.Content != null)
            {

                sb.Append($"Content ");
                foreach (var header in response.Content.Headers)
                {
                    sb.Append($"{header.Key}:{String.Join(",", header.Value)} - ");
                }
                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                sb.AppendLine($"{body} END.");
            }
            _logger.LogTrace(sb.ToString());
        }
    }
}