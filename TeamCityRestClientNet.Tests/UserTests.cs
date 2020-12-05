using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class UserTests
    {
        private readonly TeamCity _teamCity;

        public UserTests(TeamCityFixture teamCityFixture) 
            => _teamCity = teamCityFixture.TeamCity;

        [Fact]
        public async Task Users_query_returns_all_users()
        {
            var list = await _teamCity.Users.All().ToListAsync();
            Assert.Equal(2, list.Count);                
        }
    }
}
