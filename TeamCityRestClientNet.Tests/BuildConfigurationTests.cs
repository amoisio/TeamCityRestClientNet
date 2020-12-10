using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class BuildConfigurationTests : TestsBase
    {
        public BuildConfigurationTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        // [Fact]
        // public async Task VcsRoots_query_returns_all_vcsroots()
        // {
        //     var vcsRoots = await _teamCity.BuildConfiguration()
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
