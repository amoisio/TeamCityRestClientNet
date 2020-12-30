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
        private readonly FakeServer _fakeServer;
        public LoopbackHandler(FakeServer fakeServer)
        {
            _fakeServer = fakeServer;
        }

        public ApiCall ApiCall { get; set; }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.ApiCall = new ApiCall(request);
            var fs = _fakeServer;
            HttpStatusCode code;
            HttpContent content;
            try
            {
                var response = fs.ResolveApiCall(this.ApiCall);

                if (response == null)
                {
                    code = HttpStatusCode.NotFound;
                    content = null;
                }
                else
                {
                    code = HttpStatusCode.OK;
                    content = new StringContent(JsonConvert.SerializeObject(response));
                }
            }
            catch
            {
                code = HttpStatusCode.BadRequest;
                content = null;
            }

            return await Task
                    .FromResult(new HttpResponseMessage(code)
                    {
                        Content = content,
                        RequestMessage = request
                    })
                    .ConfigureAwait(false);
        }
    }
}