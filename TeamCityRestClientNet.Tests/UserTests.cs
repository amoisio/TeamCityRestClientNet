using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Users
{
    public class UserList : TestsBase
    {
        [Obsolete]
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

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/users");
            Assert.NotNull(apiCall);
        }
    }

    public class ExistingUser : TestsBase
    {
        [Obsolete]
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

        [Obsolete]
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
            await _teamCity.Users.ById(new Id("1"));

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/users/id:1");
            Assert.NotNull(apiCall);
            Assert.Equal("1", apiCall.GetLocatorValue());
        }

        [Fact]
        public async Task GETs_users_end_point_with_username_locator()
        {
            await _teamCity.Users.ByUsername("jadoe");

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/users/jadoe");
            Assert.NotNull(apiCall);
            Assert.Equal("jadoe", apiCall.GetLocatorValue("username"));
        }
    }
}
