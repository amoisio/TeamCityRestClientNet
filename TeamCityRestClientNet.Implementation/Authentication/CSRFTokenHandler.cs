using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public class CSRFTokenHandler : DelegatingHandler
    {
        private const string CSRF_HEADER = "X-TC-CSRF-Token";
        private readonly ICSRFTokenStore _csrfTokenStore;
        public CSRFTokenHandler(ICSRFTokenStore csrfTokenStore, HttpMessageHandler innerHandler = null)
           : base(innerHandler ?? new HttpClientHandler())
        {
            _csrfTokenStore = csrfTokenStore ?? throw new ArgumentNullException(nameof(csrfTokenStore));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var csrfMethods = new HttpMethod[] { HttpMethod.Put, HttpMethod.Post, HttpMethod.Delete };
            if (csrfMethods.Contains(request.Method)) {
                var csrfToken = await _csrfTokenStore.GetToken().ConfigureAwait(false);
                request.Headers.Add(CSRF_HEADER, csrfToken);
            }
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}