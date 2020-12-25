using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Tests
{
    public class LoopbackHandler: DelegatingHandler
    {
        public ApiCall ApiCall { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.ApiCall = new ApiCall(request);
            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)).ConfigureAwait(false);
        }
    }
}