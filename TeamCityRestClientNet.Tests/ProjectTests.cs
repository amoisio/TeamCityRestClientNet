using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.Projects
{
    [Collection("TeamCity Collection")]
    public class RootProject : TestsBase 
    {
        public RootProject(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_retrieved()
        {
            var rootProject = await _teamCity.RootProject();

            Assert.Equal("_Root", rootProject.Id.stringId);
            Assert.Equal("<Root project>", rootProject.Name);
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
    }

    [Collection("TeamCity Collection")]
    public class NewProject : TestsBase
    {
        public NewProject(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

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
    }

    [Collection("TeamCity Collection")]
    public class ExistingProject : TestsBase 
    {
        public ExistingProject(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var project = await _teamCity.Project(new ProjectId("TeamCityRestClientNet"));

            Assert.Equal("TeamCity Rest Client .NET", project.Name);
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
    }
}
