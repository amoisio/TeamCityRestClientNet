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

        protected void AssertApiCall(HttpMethod method, string requestPath, params Action<ApiCall>[] asserts)
        {
            var calls = ApiCalls.Where(call => call.Method == method && call.RequestPath == requestPath);
            int count = calls.Count();
            Xunit.Assert.False(count > 1, $"Multiple Api calls found for {method.ToString()} {requestPath}.");
            Xunit.Assert.False(count == 0, $"No Api calls found for {method.ToString()} {requestPath}.");

            var apiCall = calls.Single();
            foreach(var assert in asserts)
            {
                assert(apiCall);
            }
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
