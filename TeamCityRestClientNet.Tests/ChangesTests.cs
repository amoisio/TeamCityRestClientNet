using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TeamCityRestClientNet.Tests;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Changes
{
    [Collection("TeamCity Collection")]
    public class ChangeList : TestsBase 
    {
        public ChangeList(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        // TODO: implement tests once end-point is supported in client.
    }

    [Collection("TeamCity Collection")]
    public class ExistingChange : TestsBase
    {
        public ExistingChange(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var change = await _teamCity.Change(new ChangeId("1"));
            Assert.Equal("Add project properties for nuget generation.", change.Comment.Trim());
            Assert.Equal(Utilities.ParseTeamCity("20201201T155303+0000"), change.DateTime);
            Assert.Equal("1", change.Id.stringId);
            var user = await change.User;
            Assert.Equal("amoisio", user.Username);
            Assert.Equal("Aleksi Moisio", user.Name);
            Assert.Equal("1", user.Id.stringId);
            Assert.Equal("aleksi.moisio30", change.Username);
            Assert.Equal("Bitbucket", change.VcsRootInstance.Name);
            Assert.Equal("3f5917db0229261ff6c99561618155f8d35d3cf6", change.Version);
        }

        [Fact]
        public async Task Retrieval_throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Change(new ChangeId("999991")));
        }

        [Fact]
        public async Task First_builds_can_be_retrieved()
        {
            var change = await _teamCity.Change(new ChangeId("2"));

            var build = (await change.FirstBuilds()).First();
            Assert.Equal("11", build.BuildNumber);
            Assert.Equal(BuildStatus.SUCCESS, build.Status.Value);
            Assert.Equal(BuildState.FINISHED, build.State);
        } 
    }
}