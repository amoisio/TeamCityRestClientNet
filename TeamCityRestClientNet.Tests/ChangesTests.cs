using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class ChangesTests : TestsBase
    {
        public ChangesTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Changes_query_by_id_returns_information_about_the_commit()
        {
            var change = await _teamCity.Change(new ChangeId("1"));
            var user = await change.User;
            Assert.Equal("amoisio", user.Username);
            Assert.Equal("Aleksi Moisio", user.Name);
            Assert.Equal("1", user.Id.stringId);
            Assert.Equal("Add project properties for nuget generation.", change.Comment.Trim());
            Assert.Equal("aleksi.moisio30", change.Username);
            Assert.Equal("3f5917db0229261ff6c99561618155f8d35d3cf6", change.Version);
            Assert.Equal("Bitbucket", change.VcsRootInstance.Name);
        }

        [Fact]
        public async Task Changes_query_by_id_throws_ApiException_if_change_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Change(new ChangeId("999991")));
        }

        [Fact]
        public async Task FirstBuilds_of_a_change()
        {
            var change = await _teamCity.Change(new ChangeId("2"));
            var build = (await change.FirstBuilds()).First();
            Assert.Equal("11", build.BuildNumber);
            Assert.Equal(BuildStatus.SUCCESS, build.Status.Value);
            Assert.Equal(BuildState.FINISHED, build.State);
        } 
    }
}