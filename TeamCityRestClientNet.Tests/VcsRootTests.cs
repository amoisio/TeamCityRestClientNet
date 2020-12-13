using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Collections.Generic;

namespace TeamCityRestClientNet.VcsRoots
{
    [Collection("TeamCity Collection")]
    public class VcsRootList : TestsBase
    {
        public VcsRootList(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Contains_all_VcsRoots()
        {
            var vcsRoots = await _teamCity.VcsRoots().ToListAsync();
            Assert.Contains(vcsRoots, (root) => root.Id.stringId == "TeamCityRestClientNet_Bitbucket");
        }
    }

    [Collection("TeamCity Collection")]
    public class NewGitVcsRoot : TestsBase 
    {
        public NewGitVcsRoot(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_created_for_root_project()
        {
            var project = await _teamCity.RootProject();

            var vcsId = $"Vcs_{Guid.NewGuid().ToString().Replace('-', '_')}";

            await project.CreateVcsRoot(
                new VcsRootId(vcsId),
                vcsId,
                VcsRootType.GIT,
                new Dictionary<string, string>());

            var vcs = await _teamCity.VcsRoot(new VcsRootId(vcsId));
            Assert.Equal(vcsId, vcs.Id.stringId);
            Assert.Equal(vcsId, vcs.Name);
            // TODO: Add vcs root type check
            // TODO: Add parameter checks
        }

        [Fact]
        public async Task Creation_throws_ApiException_if_id_is_invalid()
        {
            var project = await _teamCity.RootProject();

            var vcsRootId = "-----TeamCityRestClientNet_Bitbucket";
            await Assert.ThrowsAsync<Refit.ApiException>(
                () => project.CreateVcsRoot(new VcsRootId(vcsRootId), vcsRootId, VcsRootType.GIT, new Dictionary<string, string>()));
        }

        [Fact]
        public async Task Creation_throws_ApiException_if_id_already_exists()
        {
            var project = await _teamCity.RootProject();

            var vcsRootId = "TeamCityRestClientNet_Bitbucket";
            await Assert.ThrowsAsync<Refit.ApiException>(
                () => project.CreateVcsRoot(new VcsRootId(vcsRootId), vcsRootId, VcsRootType.GIT, new Dictionary<string, string>()));
        }
    }

    [Collection("TeamCity Collection")]
    public class ExistingVcsRoot : TestsBase
    {
        public ExistingVcsRoot(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var rootId = new VcsRootId("TeamCityRestClientNet_Bitbucket");
            var root = await _teamCity.VcsRoot(rootId);
            Assert.Equal(rootId, root.Id);
            Assert.Equal("Bitbucket", root.Name);
            Assert.Equal("refs/heads/master", root.DefaultBranch);
            Assert.Equal("https://amoisio@bitbucket.org/amoisio/teamcityrestclientnet.git", root.Url);
        }

        [Fact]
        public async Task Retrieval_throws_ApiException_if_id_not_found()
        {
            var rootId = new VcsRootId("Not_found");

            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.VcsRoot(rootId));
        }
    }
}
