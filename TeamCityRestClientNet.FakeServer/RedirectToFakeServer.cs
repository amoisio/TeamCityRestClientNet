using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.FakeServer
{
    public class RedirectToFakeServer : DelegatingHandler
    {
        private readonly FakeServer _fakeServer;
        public RedirectToFakeServer(FakeServer fakeServer)
        {
            _fakeServer = fakeServer;
            ApiCalls = new List<ApiCall>();
        }

        public List<ApiCall> ApiCalls { get; set; }
        public ApiCall FirstApiCall => ApiCalls.First();
        public ApiCall ApiCall => ApiCalls.Last();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiCall = new ApiCall(request);
            ApiCalls.Add(apiCall);
            var fs = _fakeServer;
            HttpStatusCode code;
            HttpContent content;
            try
            {
                var response = fs.ResolveApiCall(apiCall);

                if (response == null)
                {
                    code = HttpStatusCode.NotFound;
                    content = null;
                }
                else
                {
                    code = HttpStatusCode.OK;
                    if (apiCall.RespondAsStream)
                        content = new StreamContent(response as Stream);
                    else 
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