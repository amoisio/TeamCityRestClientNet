using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;
using TeamCityRestClientNet.RestApi;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TeamCityRestClientNet.Projects
{
    public class RootProject : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_projects_end_point_with_root_id()
        {
            await _teamCity.Projects.RootProject();

            AssertApiCall(HttpMethod.Get, "/app/rest/projects/_Root");
        }

        [Fact]
        public async Task Child_projects_can_be_retrieved_by_GETting_the_projects_end_point_with_child_id()
        {
            var rootProject = await _teamCity.Projects.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var childIds = childProjects.Select(p => p.Id.StringId);

            foreach(var childId in childIds) {
                AssertApiCall(HttpMethod.Get, $"/app/rest/projects/{childId}");
            }
        }
    }

    public class NewProject : TestsBase
    {
        [Fact]
        public async Task Can_be_created_by_POSTing_the_projects_end_point_with_new_project_body_and_root_as_parent()
        {
            var project = await _teamCity.Projects.RootProject();
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";

            await project.CreateProject(new Id(projectId), projectId);

            AssertApiCall(HttpMethod.Post, "/app/rest/projects",
                apiCall => {
                    var body = apiCall.XmlContentAs<NewProjectDescriptionDto>();
                    Assert.Equal(projectId, body.Id);
                    Assert.Equal(projectId, body.Name);
                    Assert.Equal("id:_Root", body.ParentProject.Locator);
                });
        }

        [Fact]
        public async Task Can_be_created_by_POSTing_the_projects_end_point_with_new_project_body_and_project_as_parent()
        {
            var root = await _teamCity.Projects.RootProject();
            var project = (await root.ChildProjects).First();
            
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            await project.CreateProject(new Id(projectId), projectId);

            AssertApiCall(HttpMethod.Post, "/app/rest/projects",
                apiCall =>
                {
                    var body = apiCall.XmlContentAs<NewProjectDescriptionDto>();
                    Assert.Equal(projectId, body.Id);
                    Assert.Equal(projectId, body.Name);
                    Assert.Equal($"id:{project.Id}", body.ParentProject.Locator);
                });
        }
    }

    public class ExistingProject : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETtin_the_projects_end_point_with_project_id()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            AssertApiCall(HttpMethod.Get, "/app/rest/projects/TeamCityRestClientNet");
        }

        [Fact]
        public async Task Parameters_can_be_changed_by_PUTting_the_project_parameters_end_point_with_changed_value()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            var newValue = Guid.NewGuid().ToString();
            await project.SetParameter("configuration_parameter", newValue);

            AssertApiCall(HttpMethod.Put, "/app/rest/projects/TeamCityRestClientNet/parameters/configuration_parameter",
                apiCall => {
                    Assert.Equal("parameters", apiCall.Property);
                    Assert.Equal("configuration_parameter", apiCall.Descriptor);
                    Assert.Contains(newValue, apiCall.Content);
                });
        }

        [Fact]
        public async Task Can_be_deleted_by_DELETEing_the_projects_end_point_with_id_locator()
        {
            var rootProject = await _teamCity.Projects.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var toDelete = childProjects
                .First(project => project.Id.StringId.StartsWith("Project_"));

            await toDelete.Delete();

            AssertApiCall(HttpMethod.Delete, $"/app/rest/projects/id:{toDelete.Id}");
        }
    }
}
