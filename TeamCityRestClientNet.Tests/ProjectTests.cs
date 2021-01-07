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
        [Obsolete]
        [Fact]
        public async Task Can_be_retrieved()
        {
            var rootProject = await _teamCity.Projects.RootProject();

            Assert.Equal("_Root", rootProject.Id.StringId);
            Assert.Equal("<Root project>", rootProject.Name);
        }

        [Fact]
        public async Task GETs_the_projects_end_point_with_root_id()
        {
            await _teamCity.Projects.RootProject();

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/projects/_Root");
            Assert.NotNull(apiCall);
            Assert.Equal("_Root", apiCall.GetLocatorValue());
        }

        [Obsolete]
        [Fact]
        public async Task Child_projects_can_be_retrieved()
        {
            var rootProject = await _teamCity.Projects.RootProject();

            var childProjects = await rootProject.ChildProjects;

            Assert.Contains(childProjects, p => p.Name == "TeamCity CLI .NET");
            Assert.Contains(childProjects, p => p.Id.StringId == "TeamCityCliNet");
            Assert.Contains(childProjects, p => p.Name == "TeamCity Rest Client .NET");
            Assert.Contains(childProjects, p => p.Id.StringId == "TeamCityRestClientNet");
        }

        [Fact]
        public async Task GETs_the_projects_end_point_with_child_id()
        {
            var rootProject = await _teamCity.Projects.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var childIds = childProjects.Select(p => p.Id.StringId);

            foreach(var childId in childIds) {
                var apiCall = GetApiCall(HttpMethod.Get, $"/app/rest/projects/{childId}");
                Assert.NotNull(apiCall);
                Assert.Equal(childId, apiCall.GetLocatorValue());
            }
        }
    }

    public class NewProject : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_created_as_child_to_root_project()
        {
            var project = await _teamCity.Projects.RootProject();
            
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            var returnedProject = await project.CreateProject(new Id(projectId), projectId);

            var newProject = await _teamCity.Projects.ById(new Id(projectId));
            Assert.NotNull(newProject);
            Assert.Equal(projectId, newProject.Name);
            Assert.Equal(projectId, returnedProject.Name);
        }

        [Fact]
        public async Task POSTs_the_projects_end_point_with_new_project_body_and_root_as_parent()
        {
            var project = await _teamCity.Projects.RootProject();
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";

            await project.CreateProject(new Id(projectId), projectId);

            NewProjectDescriptionDto body;
            var bodyStr = ApiCall.Content;
            using (var sr = new StringReader(bodyStr))
            using (var xr = XmlReader.Create(sr))
            {
                var serializer = new XmlSerializer(typeof(NewProjectDescriptionDto));
                body = serializer.Deserialize(xr) as NewProjectDescriptionDto;
            }

            var apiCall = GetApiCall(HttpMethod.Post, "/app/rest/projects");
            Assert.NotNull(apiCall);
            Assert.Equal(projectId, body.Id);
            Assert.Equal(projectId, body.Name);
            Assert.Equal("id:_Root", body.ParentProject.Locator);
        }

        [Fact]
        public async Task Creation_throws_ApiException_if_id_is_invalid()
        {
            var project = await _teamCity.Projects.RootProject();

            var projectId = $"---{Guid.NewGuid().ToString().Replace('-', '_')}";
            await Assert.ThrowsAsync<Refit.ApiException>(() => project.CreateProject(new Id(projectId), projectId));
        }

        [Fact]
        public async Task Creation_throws_ApiException_if_id_already_exists()
        {
            var project = await _teamCity.Projects.RootProject();

            var projectId = "TeamCityRestClientNet";
            await Assert.ThrowsAsync<Refit.ApiException>(() => project.CreateProject(new Id(projectId), projectId));
        }

        [Obsolete]
        [Fact]
        public async Task Can_be_created_as_child_to_nonroot_project()
        {
            var root = await _teamCity.Projects.RootProject();
            var project = (await root.ChildProjects).First();

            var newProjectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            await project.CreateProject(new Id(newProjectId), newProjectId);

            project = await _teamCity.Projects.ById(project.Id);
            var childProjects = await project.ChildProjects;
            Assert.Contains(childProjects, p => p.Name == newProjectId);
        }

        [Fact]
        public async Task POSTs_the_projects_end_point_with_new_project_body_and_project_as_parent()
        {
            var root = await _teamCity.Projects.RootProject();
            var project = (await root.ChildProjects).First();
            
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            await project.CreateProject(new Id(projectId), projectId);

            NewProjectDescriptionDto body;
            var bodyStr = ApiCall.Content;
            using (var sr = new StringReader(bodyStr))
            using (var xr = XmlReader.Create(sr))
            {
                var serializer = new XmlSerializer(typeof(NewProjectDescriptionDto));
                body = serializer.Deserialize(xr) as NewProjectDescriptionDto;
            }

            var apiCall = GetApiCall(HttpMethod.Post, $"/app/rest/projects");
            Assert.NotNull(apiCall);
            Assert.Equal(projectId, body.Id);
            Assert.Equal(projectId, body.Name);
            Assert.Equal($"id:{project.Id}", body.ParentProject.Locator);
        }
    }

    public class ExistingProject : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            Assert.Equal("TeamCity Rest Client .NET", project.Name);
        }

        [Fact]
        public async Task GETs_the_projects_end_point_with_project_id()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/projects/TeamCityRestClientNet");
            Assert.NotNull(apiCall);
            Assert.Equal("TeamCityRestClientNet", project.Id.StringId);
        }

        [Fact]
        public async Task Retrieval_throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Projects.ById(new Id("Not.Found")));
        }

        [Obsolete]
        [Fact]
        public async Task Parameters_can_be_retrieved()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            Assert.Collection(project.Parameters,
                (param) => Assert.Equal("configuration_parameter", param.Name));
        }

        [Obsolete]
        [Fact]
        public async Task Existing_parameter_value_can_be_changed()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            var newValue = Guid.NewGuid().ToString();
            await project.SetParameter("configuration_parameter", newValue);

            project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));
            Assert.Collection(project.Parameters,
                (param) => {
                    Assert.Equal("configuration_parameter", param.Name);
                    Assert.Equal(newValue, param.Value);
                });
        }

        [Fact]
        public async Task PUTs_the_project_parameters_end_point_with_changed_value()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            var newValue = Guid.NewGuid().ToString();
            await project.SetParameter("configuration_parameter", newValue);

            var apiCall = GetApiCall(HttpMethod.Put, "/app/rest/projects/TeamCityRestClientNet/parameters/configuration_parameter");
            Assert.NotNull(apiCall);
            Assert.Equal("TeamCityRestClientNet", apiCall.GetLocatorValue());
            Assert.Equal("parameters", apiCall.Property);
            Assert.Equal("configuration_parameter", apiCall.Descriptor);
            Assert.Contains(newValue, ApiCall.Content);
        }

        [Obsolete]
        [Fact]
        public async Task Can_be_deleted() 
        {
            var rootProject = await _teamCity.Projects.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var toDelete = childProjects
                .First(project => project.Id.StringId.StartsWith("Project_"));

            await toDelete.Delete();

            rootProject = await _teamCity.Projects.RootProject();
            childProjects = await rootProject.ChildProjects;
            
            Assert.DoesNotContain(childProjects, project => project.Id.StringId == toDelete.Id.StringId);
        }

        [Fact]
        public async Task DELETEs_the_project_root_with_id_locator()
        {
            var rootProject = await _teamCity.Projects.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var toDelete = childProjects
                .First(project => project.Id.StringId.StartsWith("Project_"));

            await toDelete.Delete();

            var apiCall = GetApiCall(HttpMethod.Delete, $"/app/rest/projects/id:{toDelete.Id}");
            Assert.NotNull(apiCall);
            Assert.True(apiCall.HasLocators);
            Assert.Equal(toDelete.Id.StringId, apiCall.GetLocatorValue());
        }
    }
}
