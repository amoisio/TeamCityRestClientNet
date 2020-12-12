using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class UserTests : TestsBase
    {
        public UserTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Users_query_returns_all_users()
        {
            var users = await _teamCity.Users().ToListAsync();

            Assert.Collection(users, 
                user => Assert.Equal("amoisio", user.Username),
                user => Assert.Equal("jbuilder", user.Username));
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
        public async Task User_query_by_id_throws_an_ApiException_if_user_not_found()
        {
            var userId = new UserId("9999");
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.User(userId));
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

        [Fact]
        public async Task User_query_username_id_throws_an_ApiException_if_user_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.User("not.found"));
        }
    }
}
