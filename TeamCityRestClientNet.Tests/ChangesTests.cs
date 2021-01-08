using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Changes
{
    public class ChangeList : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Contains_all_changes()
        {
            var changes = await _teamCity.Changes.All().ToListAsync();

            Assert.Contains(changes, change => change.Id.StringId == "1" && change.Username == "jodoe");
            Assert.Contains(changes, change => change.Id.StringId == "2" && change.Username == "jodoe");
        }

        [Fact]
        public async Task GETs_the_changes_end_point()
        {
            await _teamCity.Changes.All().ToListAsync();

            var apiCall = ApiCall(HttpMethod.Get, "/app/rest/changes");
            Assert.NotNull(apiCall);
        }
    }

    public class ExistingChange : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var change = await _teamCity.Changes.ById(new Id("1"));
            Assert.Equal("Initial commit", change.Comment.Trim());
            Assert.Equal("1", change.Id.StringId);
            var user = await change.User;
            Assert.Equal("jodoe", user.Username);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("1", user.Id.StringId);
            Assert.Equal("jodoe", change.Username);
            var rootInstance = await change.VcsRootInstance;
            Assert.Equal("Bitbucket", rootInstance.Name);
            Assert.Equal("a9f57192-48d1-4e7a-b3f5-ebead0c6f8d6", change.Version);
        }

        [Fact]
        public async Task GETs_the_changes_end_point_with_id()
        {
            var change = await _teamCity.Changes.ById(new Id("1"));

            var apiCall = ApiCall(HttpMethod.Get, "/app/rest/changes/1");
            Assert.NotNull(apiCall);
            Assert.True(apiCall.HasLocators);
            Assert.Equal("1", apiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Changes.ById(new Id("999991")));
        }
    }
}