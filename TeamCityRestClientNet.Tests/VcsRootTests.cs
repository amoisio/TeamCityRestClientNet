using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class VcsRootTests : TestsBase
    {
        public VcsRootTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task VcsRoots_query_returns_all_vcsroots()
        {
            var vcsRoots = await _teamCity.VcsRoots().ToListAsync();
            Assert.Contains(vcsRoots, (root) => root.Id.stringId == "TeamCityRestClientNet_Bitbucket");
        }

        [Fact]
        public async Task VcsRoot_query_returns_the_matching_vcsroot()
        {
            var rootId = new VcsRootId("TeamCityRestClientNet_Bitbucket");
            var root = await _teamCity.VcsRoot(rootId);
            Assert.Equal(rootId, root.Id);
            Assert.Equal("Bitbucket", root.Name);
            Assert.Equal("refs/heads/master", root.DefaultBranch);
            Assert.Equal("https://amoisio@bitbucket.org/amoisio/teamcityrestclientnet.git", root.Url);
        }

        [Fact]
        public async Task VcsRoot_query_throws_ApiException_if_vcsroot_not_found()
        {
            var rootId = new VcsRootId("Not_found");

            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.VcsRoot(rootId));
        }
    }
}
