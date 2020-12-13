using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.Users
{
    [Collection("TeamCity Collection")]
    public class UserList : TestsBase
    {
        public UserList(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Contains_all_users()
        {
            var users = await _teamCity.Users().ToListAsync();

            Assert.Collection(users,
                user => Assert.Equal("amoisio", user.Username),
                user => Assert.Equal("jbuilder", user.Username));
        }
    }


    [Collection("TeamCity Collection")]
    public class NewUser : TestsBase
    {
        public NewUser(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        // [Fact]
        // public async Task Can_be_created() 
        // {

        // }
    }

    [Collection("TeamCity Collection")]
    public class ExistingUser : TestsBase
    {
        public ExistingUser(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
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
        public async Task Throws_ApiException_if_id_not_found()
        {
            var userId = new UserId("9999");
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.User(userId));
        }

        [Fact]
        public async Task Can_be_retrieved_with_exact_username()
        {
            var user = await _teamCity.User("amoisio");
            Assert.Equal(new UserId("1"), user.Id);
            Assert.Equal("aleksi.moisio30@gmail.com", user.Email);
            Assert.Equal("Aleksi Moisio", user.Name);
            Assert.Equal("amoisio", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=1", user.GetHomeUrl());
        }

        [Fact]
        public async Task Throws_ApiException_if_exact_username_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.User("not.found"));
        }
    }
}
