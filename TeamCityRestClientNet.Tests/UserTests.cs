using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Users
{
    public class UserList : TestsBase, IClassFixture<TeamCityFixture>
    {
        public UserList(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Contains_all_users()
        {
            var users = await _teamCity.Users.All().ToListAsync();

            Assert.Collection(users,
                user => Assert.Equal("jodoe", user.Username),
                user => Assert.Equal("jadoe", user.Username),
                user => Assert.Equal("dunkin", user.Username),
                user => Assert.Equal("maccheese", user.Username));
        }

        [Fact]
        public async Task GETs_the_users_end_point()
        {
            var users = await _teamCity.Users.All().ToListAsync();

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/users", ApiCall.RequestPath);
        }
    }

    public class NewUser : TestsBase, IClassFixture<TeamCityFixture>
    {
        public NewUser(TeamCityFixture fixture) : base(fixture) { }

        // [Fact]
        // public async Task Can_be_created() 
        // {

        // }
    }

    public class ExistingUser : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ExistingUser(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var userId = new Id("1");
            var user = await _teamCity.Users.ById(userId);
            Assert.Equal(userId, user.Id);
            Assert.Equal("john.doe@mailinator.com", user.Email);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("jodoe", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=1", user.GetHomeUrl());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            var userId = new Id("9999");
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Users.ById(userId));
        }

        [Fact]
        public async Task Can_be_retrieved_with_exact_username()
        {
            var user = await _teamCity.Users.ByUsername("dunkin");
            Assert.Equal(new Id("3"), user.Id);
            Assert.Equal("dunkin@mailinator.com", user.Email);
            Assert.Equal("Dunkin' Donuts", user.Name);
            Assert.Equal("dunkin", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=3", user.GetHomeUrl());
        }

        [Fact]
        public async Task Throws_ApiException_if_exact_username_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Users.ByUsername("not.found"));
        }

        [Fact]
        public async Task GETs_users_end_point_with_id_locator()
        {
            var user = await _teamCity.Users.ById(new Id("1"));

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/users", ApiCall.RequestPath);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal("1", ApiCall.GetLocatorValue());
        }

        [Fact]
        public async Task GETs_users_end_point_with_username_locator()
        {
            var user = await _teamCity.Users.ByUsername("jadoe");

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/users", ApiCall.RequestPath);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal("jadoe", ApiCall.GetLocatorValue("username"));
        }
    }
}
