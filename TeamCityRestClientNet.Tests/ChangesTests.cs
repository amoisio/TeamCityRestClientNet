using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Changes
{
    public class ChangeList : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ChangeList(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Contains_all_changes()
        {
            var changes = await _teamCity.Changes.All().ToListAsync();

            Assert.Contains(changes, change => change.Id.stringId == "1" && change.Username == "jodoe");
            Assert.Contains(changes, change => change.Id.stringId == "2" && change.Username == "jodoe");
        }

        [Fact]
        public async Task GETs_the_changes_end_point()
        {
            await _teamCity.Changes.All().ToListAsync();

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/changes", ApiCall.RequestPath);
        }
    }

    public class ExistingChange : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ExistingChange(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var change = await _teamCity.Changes.Change(new ChangeId("1"));
            Assert.Equal("Initial commit", change.Comment.Trim());
            Assert.Equal("1", change.Id.stringId);
            var user = await change.User;
            Assert.Equal("jodoe", user.Username);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("1", user.Id.StringId);
            Assert.Equal("jodoe", change.Username);
            Assert.Equal("Bitbucket", change.VcsRootInstance.Name);
            Assert.Equal("a9f57192-48d1-4e7a-b3f5-ebead0c6f8d6", change.Version);
        }

        [Fact]
        public async Task GETs_the_changes_end_point_with_id()
        {
            var change = await _teamCity.Changes.Change(new ChangeId("1"));

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/changes", ApiCall.RequestPath);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal("1", ApiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Changes.Change(new ChangeId("999991")));
        }
    }
}