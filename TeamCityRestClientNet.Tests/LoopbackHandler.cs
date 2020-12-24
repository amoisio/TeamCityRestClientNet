using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Tests
{
    public class LoopbackHandler: DelegatingHandler
    {
        public HttpMethod Method { get; private set; }
        public string RequestPath { get; private set; }
        public Dictionary<string, string[]> QueryParameters { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.Method = request.Method;
            this.RequestPath = request.RequestUri.ToString();
            this.QueryParameters = new Dictionary<string, string[]>();
            
            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)).ConfigureAwait(false);
        }
    }
}