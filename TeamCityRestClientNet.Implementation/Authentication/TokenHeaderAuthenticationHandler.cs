using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public class TokenHeaderAuthenticationHandler : DelegatingHandler
    {
        private readonly IAuthTokenStore authTokenStore;

        public TokenHeaderAuthenticationHandler(IAuthTokenStore authTokenStore, HttpMessageHandler innerHandler = null)
           : base(innerHandler ?? new HttpClientHandler())
        {
            this.authTokenStore = authTokenStore ?? throw new ArgumentNullException(nameof(authTokenStore));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await authTokenStore.GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}