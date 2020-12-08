using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class BuildQueueTests
    {
        private readonly TeamCity _teamCity;
        private readonly string _serverUrl;

        public BuildQueueTests(TeamCityFixture teamCityFixture)
        {
            _teamCity = teamCityFixture.TeamCity;
            _serverUrl = teamCityFixture.serverUrl;
        }

        // [Fact]
        // public async Task VcsRoots_query_returns_all_vcsroots()
        // {
        //     var vcsRoots = await _teamCity.QueuedBuilds()
        //     Assert.NotEmpty(vcsRoots);
        // }

        // [Fact]
        // public async Task VcsRoot_query_returns_the_matching_vcsroot()
        // {
        //     var rootId = new VcsRootId("TeamCityRestClientNet_Bitbucket");
        //     var root = await _teamCity.VcsRoot(rootId);
        //     Assert.Equal(rootId, root.Id);
        //     Assert.Equal("Bitbucket", root.Name);
        //     Assert.Equal("refs/heads/master", root.DefaultBranch);
        //     Assert.Equal("https://amoisio@bitbucket.org/amoisio/teamcityrestclientnet.git", root.Url);
        // }
    }
}
