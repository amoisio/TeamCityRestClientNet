using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Collections.Generic;

namespace TeamCityRestClientNet.VcsRoots
{
    public class VcsRootList : TestsBase, IClassFixture<TeamCityFixture>
    {
        public VcsRootList(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Contains_all_VcsRoots()
        {
            var vcsRoots = await _teamCity.VcsRoots().ToListAsync();
            Assert.Contains(vcsRoots, (root) => root.Id.stringId == "TeamCityRestClientNet_Bitbucket");
        }
    }

    public class NewGitVcsRoot : TestsBase, IClassFixture<TeamCityFixture> 
    {
        public NewGitVcsRoot(TeamCityFixture fixture) : base(fixture) { }

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

    public class ExistingVcsRoot : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ExistingVcsRoot(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var rootId = new VcsRootId("TeamCityRestClientNet_Bitbucket");
            var root = await _teamCity.VcsRoot(rootId);
            Assert.Equal(rootId, root.Id);
            Assert.Equal("Bitbucket", root.Name);
            Assert.Equal("refs/heads/master", root.DefaultBranch);
            Assert.Equal("https://noexist@bitbucket.org/joedoe/teamcityrestclientnet.git", root.Url);
        }

        [Fact]
        public async Task Retrieval_throws_ApiException_if_id_not_found()
        {
            var rootId = new VcsRootId("Not_found");

            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.VcsRoot(rootId));
        }

        [Fact]
        public async Task Can_be_deleted()
        {
            var vcsRoots = await _teamCity.VcsRoots().ToListAsync();

            var toDelete = vcsRoots
                .First(root => root.Id.stringId.StartsWith("Vcs_"));

            await toDelete.Delete();

            vcsRoots = await _teamCity.VcsRoots().ToListAsync();
            
            Assert.DoesNotContain(vcsRoots, root => root.Id.stringId == toDelete.Id.stringId);
        }
    }
}
