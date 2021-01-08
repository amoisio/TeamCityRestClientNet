using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    public class BuildTypeList : TestsBase
    {
        // [Fact]
        // public async Task Contains_all_build_configurations()
        // {
        //     var vcsRoots = await _teamCity.VcsRoots.All().ToListAsync();
        //     Assert.Contains(vcsRoots, (root) => root.Id.stringId == "TeamCityRestClientNet_Bitbucket");
        // }

        // [Fact]
        // public async Task GETs_the_vcsroots_end_point()
        // {
        //     var users = await _teamCity.VcsRoots.All().ToListAsync();

        //     Assert.Equal(HttpMethod.Get, ApiCall.Method);
        //     Assert.StartsWith("/app/rest/vcs-roots", ApiCall.RequestPath);
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

    public class ExistingBuildType : TestsBase
    {
        // [Fact]
        // public async Task Can_be_retrieved_with_id()
        // {
        //     _teamCity.BuildType()
        // }
    }
}
