using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private const string BEARER_HEADER = "Bearer";

        private readonly IBearerTokenStore _bearerTokenStore;
        public BearerTokenHandler(IBearerTokenStore authTokenStore, HttpMessageHandler innerHandler = null)
           : base(innerHandler ?? new HttpClientHandler())
        {
            _bearerTokenStore = authTokenStore ?? throw new ArgumentNullException(nameof(authTokenStore));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _bearerTokenStore.GetToken().ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue(BEARER_HEADER, token);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}