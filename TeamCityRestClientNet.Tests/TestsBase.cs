using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.FakeServer;

namespace TeamCityRestClientNet.Tests
{
    public abstract class _TestsBase
    {
        protected readonly TeamCity _teamCity;
        protected readonly string _serverUrl;

        public _TestsBase(_TeamCityFixture teamCityFixture)
        {
            _teamCity = teamCityFixture.TeamCity;
            _serverUrl = teamCityFixture.serverUrl;
        }
    }

    public abstract class TestsBase
    {
        protected readonly TeamCityFixture _fixture;
        protected readonly TeamCity _teamCity;
        protected readonly string _serverUrl;
        protected readonly RedirectToFakeServer _handler;
        protected ApiCall ApiCall => _handler.ApiCall;
        protected ApiCall FirstApiCall => _handler.FirstApiCall;
        protected List<ApiCall> ApiCalls => _handler.ApiCalls;
        protected ApiCall GetApiCall(HttpMethod method, string requestPath, Func<ApiCall, bool> filter = null)
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
