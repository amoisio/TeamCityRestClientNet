using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public class BearerAuthenticationHandler : DelegatingHandler
    {
        private const string ORIGIN_HEADER = "Origin";
        private readonly string _origin;
        private const string BEARER_HEADER = "Bearer";
        private readonly string _bearerToken;

        public BearerAuthenticationHandler(string origin, string token, HttpMessageHandler innerHandler = null)
           : base(innerHandler ?? new HttpClientHandler())
        {
            _bearerToken = token ?? throw new ArgumentNullException(nameof(token));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(BEARER_HEADER, _bearerToken);
            request.Headers.Add(ORIGIN_HEADER, _origin);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}