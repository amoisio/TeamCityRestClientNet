using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.FakeServer;

namespace TeamCityRestClientNet.Tests
{
    public abstract class TestsBase
    {
        protected readonly TeamCityFixture _fixture;
        protected readonly TeamCity _teamCity;
        protected readonly string _serverUrl;
        protected readonly RedirectToFakeServer _handler;
        private List<ApiCall> ApiCalls => _handler.ApiCalls;
        protected ApiCall ApiCall(HttpMethod method, string requestPath, Func<ApiCall, bool> filter = null)
        {
            var calls = ApiCalls.Where(call => call.Method == method && call.RequestPath == requestPath);
            if (filter != null)
                calls = calls.Where(filter);
            return calls.SingleOrDefault();
        }

        public TestsBase() : this(new TeamCityFixture()) { }

        public TestsBase(TeamCityFixture fixture)
        {
            _fixture = fixture;
            _teamCity = fixture.TeamCity;
            _serverUrl = fixture.serverUrl;
            _handler = fixture.Handler;
        }
    }
}
