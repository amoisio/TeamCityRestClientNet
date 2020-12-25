using System.Threading.Tasks;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Users
{
    public class UserQuery: IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly TeamCity _teamCity;
        private readonly string _serverUrl;
        private readonly LoopbackHandler _handler;
        private ApiCall ApiCall => _handler.ApiCall;

        public UserQuery(TestFixture teamCityFixture)
        {
            _fixture = teamCityFixture;
            _teamCity = teamCityFixture.TeamCity;
            _serverUrl = teamCityFixture.serverUrl;
            _handler = teamCityFixture.Handler;
        }
        

        [Fact]
        public async Task By_id_generates_a_call_to_users_end_point_with_id_locator()
        {
            await _fixture.CallSafe(async () => await _teamCity.User(new UserId("1")));

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/users", ApiCall.RequestPath);
            Assert.Contains(ApiCall.Locators, 
                locator => locator.Key == "id" && locator.Value == "1");
        }

        [Fact]
        public async Task By_username_generates_a_call_to_users_end_point_with_username_locator()
        {
            await _fixture.CallSafe(async () => await _teamCity.User("jane.doe"));

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/users", ApiCall.RequestPath);
            Assert.Contains(ApiCall.Locators,
                locator => locator.Key == "username" && locator.Value == "jane.doe");
        }

    }
}