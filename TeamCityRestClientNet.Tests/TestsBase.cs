using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Tests
{
    public abstract class TestsBase
    {
        protected readonly TeamCity _teamCity;
        protected readonly string _serverUrl;

        public TestsBase(TeamCityFixture teamCityFixture)
        {
            _teamCity = teamCityFixture.TeamCity;
            _serverUrl = teamCityFixture.serverUrl;
        }
    }
}
