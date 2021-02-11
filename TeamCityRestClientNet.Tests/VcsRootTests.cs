using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Collections.Generic;
using System.Net.Http;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.VcsRoots
{
    public class VcsRoots : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_vcsroots_end_point()
        {
            await _teamCity.VcsRoots.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/vcs-roots");
        }
    }

    public class NewGitVcsRoot : TestsBase
    {
        [Fact]
        public async Task Can_be_created_by_POSTing_the_vcsroots_end_point_with_new_vcs_root_body()
        {
            var project = await _teamCity.Projects.RootProject();

            var vcsId = $"Vcs_{Guid.NewGuid().ToString().Replace('-', '_')}";

            await project.CreateVcsRoot(
                new Id(vcsId),
                vcsId,
                VcsRootType.GIT,
                new Dictionary<string, string>());

            AssertApiCall(HttpMethod.Post, "/app/rest/vcs-roots",
                apiCall => {
                    var body = apiCall.XmlContentAs<NewVcsRoot>();
                    Assert.Equal(vcsId, body.Id);
                    Assert.Equal(vcsId, body.Name);
                    Assert.Equal(project.Id.StringId, body.Project.Id);
                });
        }
    }

    public class ExistingVcsRoot : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETtin_vcsroot_end_point_with_id_locator()
        {
            var rootId = new Id("TeamCityRestClientNet_Bitbucket");
            await _teamCity.VcsRoots.ById(rootId);

            AssertApiCall(HttpMethod.Get, "/app/rest/vcs-roots/TeamCityRestClientNet_Bitbucket");
        }

        [Fact]
        public async Task Can_be_deleted_by_DELETEing_vcsroot_end_point_with_id_locator()
        {
            var vcsRoots = await _teamCity.VcsRoots.All().ToListAsync();

            var toDelete = vcsRoots
                .First(root => root.Id.StringId.StartsWith("Vcs_"));
                
            await toDelete.Delete();

            AssertApiCall(HttpMethod.Delete, $"/app/rest/vcs-roots/id:{toDelete.Id}");
        }
    }
}
