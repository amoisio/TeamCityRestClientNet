using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.FakeServer
{
    public class LoopbackHandler : DelegatingHandler
    {
        public ApiCall ApiCall { get; set; }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.ApiCall = new ApiCall(request);
            var fs = new FakeServer();
            var response = fs.ResolveApiCall(this.ApiCall);
            return await Task
                .FromResult(new HttpResponseMessage(HttpStatusCode.OK) 
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                })
                .ConfigureAwait(false);
        }
    }
}