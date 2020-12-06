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
        private readonly string _serverUrl;

        public UserTests(TeamCityFixture teamCityFixture) {
            _teamCity = teamCityFixture.TeamCity;
            _serverUrl = teamCityFixture.serverUrl;
        }

        [Fact]
        public async Task Users_query_returns_all_users()
        {
            var users = await _teamCity.Users().ToListAsync();
            Assert.NotEmpty(users);
        }

        [Fact]
        public async Task User_query_by_id_returns_the_matching_user()
        {
            var userId = new UserId("2");
            var user = await _teamCity.User(userId);
            Assert.Equal(userId, user.Id);
            Assert.Equal("jbuilder@mailinator.com", user.Email);
            Assert.Equal("Jenny Builder", user.Name);
            Assert.Equal("jbuilder", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=2", user.GetHomeUrl());
        }

        [Fact]
        public async Task User_query_by_username_returns_the_matching_user()
        {
            var user = await _teamCity.User("amoisio");
            Assert.Equal(new UserId("1"), user.Id);
            Assert.Equal("aleksi.moisio30@gmail.com", user.Email);
            Assert.Equal("Aleksi Moisio", user.Name);
            Assert.Equal("amoisio", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=1", user.GetHomeUrl());
        }
    }
}
