using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
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
    public class RootProject : TestsBase, IClassFixture<TeamCityFixture>
    {
        public RootProject(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved()
        {
            var rootProject = await _teamCity.RootProject();

            Assert.Equal("_Root", rootProject.Id.stringId);
            Assert.Equal("<Root project>", rootProject.Name);
        }

        [Fact]
        public async Task GETs_the_projects_end_point_with_root_id()
        {
            var users = await _teamCity.RootProject();

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/projects", ApiCall.RequestPath);
            Assert.Equal("_Root", ApiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Child_projects_can_be_retrieved()
        {
            var rootProject = await _teamCity.RootProject();

            var childProjects = await rootProject.ChildProjects;

            Assert.Contains(childProjects, p => p.Name == "TeamCity CLI .NET");
            Assert.Contains(childProjects, p => p.Id.stringId == "TeamCityCliNet");
            Assert.Contains(childProjects, p => p.Name == "TeamCity Rest Client .NET");
            Assert.Contains(childProjects, p => p.Id.stringId == "TeamCityRestClientNet");
        }

        [Fact]
        public async Task GETs_the_projects_end_point_with_child_id()
        {
            var rootProject = await _teamCity.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var childIds = childProjects.Select(p => p.Id.stringId);

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/projects", ApiCall.RequestPath);
            Assert.Contains(ApiCall.GetLocatorValue(), childIds);
        }
    }

    public class NewProject : TestsBase, IClassFixture<TeamCityFixture>
    {
        public NewProject(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_created_as_child_to_root_project()
        {
            var project = await _teamCity.RootProject();
            
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            var returnedProject = await project.CreateProject(new ProjectId(projectId), projectId);

            var newProject = await _teamCity.Project(new ProjectId(projectId));
            Assert.NotNull(newProject);
            Assert.Equal(projectId, newProject.Name);
            Assert.Equal(projectId, returnedProject.Name);
        }

        [Fact]
        public async Task POSTs_the_projects_end_point_with_new_project_body_and_root_as_parent()
        {
            var project = await _teamCity.RootProject();
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";

            await project.CreateProject(new ProjectId(projectId), projectId);

            NewProjectDescriptionDto body;
            var bodyStr = ApiCall.Content;
            using (var sr = new StringReader(bodyStr))
            using (var xr = XmlReader.Create(sr))
            {
                var serializer = new XmlSerializer(typeof(NewProjectDescriptionDto));
                body = serializer.Deserialize(xr) as NewProjectDescriptionDto;
            }

            Assert.Equal(HttpMethod.Post, ApiCall.Method);
            Assert.StartsWith("/app/rest/project", ApiCall.RequestPath);
            Assert.Equal(projectId, body.Id);
            Assert.Equal(projectId, body.Name);
            Assert.Equal("id:_Root", body.ParentProject.Locator);
        }

        [Fact]
        public async Task Creation_throws_ApiException_if_id_is_invalid()
        {
            var project = await _teamCity.RootProject();

            var projectId = $"---{Guid.NewGuid().ToString().Replace('-', '_')}";
            await Assert.ThrowsAsync<Refit.ApiException>(() => project.CreateProject(new ProjectId(projectId), projectId));
        }

        [Fact]
        public async Task Creation_throws_ApiException_if_id_already_exists()
        {
            var project = await _teamCity.RootProject();

            var projectId = "TeamCityRestClientNet";
            await Assert.ThrowsAsync<Refit.ApiException>(() => project.CreateProject(new ProjectId(projectId), projectId));
        }

        [Fact]
        public async Task Can_be_created_as_child_to_nonroot_project()
        {
            var root = await _teamCity.RootProject();
            var project = (await root.ChildProjects).First();

            var newProjectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            await project.CreateProject(new ProjectId(newProjectId), newProjectId);

            project = await _teamCity.Project(project.Id);
            var childProjects = await project.ChildProjects;
            Assert.Contains(childProjects, p => p.Name == newProjectId);
        }

        [Fact]
        public async Task POSTs_the_projects_end_point_with_new_project_body_and_project_as_parent()
        {
            var root = await _teamCity.RootProject();
            var project = (await root.ChildProjects).First();
            
            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            await project.CreateProject(new ProjectId(projectId), projectId);

            NewProjectDescriptionDto body;
            var bodyStr = ApiCall.Content;
            using (var sr = new StringReader(bodyStr))
            using (var xr = XmlReader.Create(sr))
            {
                var serializer = new XmlSerializer(typeof(NewProjectDescriptionDto));
                body = serializer.Deserialize(xr) as NewProjectDescriptionDto;
            }

            Assert.Equal(HttpMethod.Post, ApiCall.Method);
            Assert.StartsWith("/app/rest/project", ApiCall.RequestPath);
            Assert.Equal(projectId, body.Id);
            Assert.Equal(projectId, body.Name);
            Assert.Equal($"id:{project.Id}", body.ParentProject.Locator);
        }

    }

    public class ExistingProject : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ExistingProject(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));

            Assert.Equal("TeamCity Rest Client .NET", project.Name);
        }

        [Fact]
        public async Task GETs_the_projects_end_point_with_project_id()
        {
            var project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/projects", ApiCall.RequestPath);
            Assert.Equal("TeamCityRestClientNet", project.Id.stringId);
        }

        [Fact]
        public async Task Retrieval_throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Project(new ProjectId("Not.Found")));
        }

        [Fact]
        public async Task Parameters_can_be_retrieved()
        {
            var project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));

            Assert.Collection(project.Parameters,
                (param) => Assert.Equal("configuration_parameter", param.Name));
        }

        [Fact]
        public async Task Existing_parameter_value_can_be_changed()
        {
            var project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));

            var newValue = Guid.NewGuid().ToString();
            await project.SetParameter("configuration_parameter", newValue);

            project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));
            Assert.Collection(project.Parameters,
                (param) => {
                    Assert.Equal("configuration_parameter", param.Name);
                    Assert.Equal(newValue, param.Value);
                });
        }

        [Fact]
        public async Task PUTs_the_project_parameters_end_point_with_changed_value()
        {
            var project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));

            var newValue = Guid.NewGuid().ToString();
            await project.SetParameter("configuration_parameter", newValue);

            Assert.Equal(HttpMethod.Put, ApiCall.Method);
            Assert.StartsWith("/app/rest/projects/TeamCityRestClientNet/parameters/configuration_parameter", ApiCall.RequestPath);
            Assert.Equal("TeamCityRestClientNet", ApiCall.GetLocatorValue());
            Assert.Equal("parameters", ApiCall.Property);
            Assert.Equal("configuration_parameter", ApiCall.Descriptor);
            Assert.Contains(newValue, ApiCall.Content);
        }


        [Fact]
        public async Task Can_be_deleted() 
        {
            var rootProject = await _teamCity.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var toDelete = childProjects
                .First(project => project.Id.stringId.StartsWith("Project_"));

            await toDelete.Delete();

            rootProject = await _teamCity.RootProject();
            childProjects = await rootProject.ChildProjects;
            
            Assert.DoesNotContain(childProjects, project => project.Id.stringId == toDelete.Id.stringId);
        }

        [Fact]
        public async Task DELETEs_the_project_root_with_id_locator()
        {
            var rootProject = await _teamCity.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var toDelete = childProjects
                .First(project => project.Id.stringId.StartsWith("Project_"));

            await toDelete.Delete();

            Assert.Equal(HttpMethod.Delete, ApiCall.Method);
            Assert.StartsWith("/app/rest/projects", ApiCall.RequestPath);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal(toDelete.Id.stringId, ApiCall.GetLocatorValue());
        }
    }
}
