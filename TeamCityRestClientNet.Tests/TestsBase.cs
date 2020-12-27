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
        protected readonly LoopbackHandler _handler;
        protected ApiCall ApiCall => _handler.ApiCall;

        public TestsBase(TeamCityFixture fixture)
        {
            _fixture = fixture;
            _teamCity = fixture.TeamCity;
            _serverUrl = fixture.serverUrl;
            _handler = fixture.Handler;
        }
    }
}
