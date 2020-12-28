using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class ProjectRepository
    {
        public static ProjectDto RootProject = new ProjectDto
        {
            Id = "_Root",
            Name = "<Root project>",
            Projects = new ProjectsDto
            {
                Project = new List<ProjectDto>
                {
                    RestClientProject,
                    TeamCityCliProject
                }
            }
        };

        public static ProjectDto RestClientProject = new ProjectDto
        {
            Id = "TeamCityRestClientNet",
            ParentProjectId = "_Root",
            Name = "TeamCity Rest Client .NET",
            Parameters = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("configuration_parameter", "6692e7bf-c9a4-4941-9e89-5dde9417f05f"),
                }
            },
            BuildTypes = new BuildTypesDto
            {
                BuildType = new List<BuildTypeDto>
                {
                    BuildTypeRepository.RestClient
                }
            }

        };

        public static ProjectDto TeamCityCliProject = new ProjectDto
        {
            Id = "TeamCityCliNet",
            ParentProjectId = "_Root",
            Name = "TeamCity CLI .NET",
            BuildTypes = new BuildTypesDto
            {
                BuildType = new List<BuildTypeDto>
                {
                    BuildTypeRepository.TeamCityCli
                }
            }
        };

        private static readonly List<ProjectDto> _projects = new List<ProjectDto>
        {
            RootProject,
            RestClientProject,
            TeamCityCliProject
        };

        static ProjectRepository()
        {
            var project1 = new ProjectDto
            {
                Id = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4",
                ParentProjectId = "_Root",
                Name = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4"
            };
            RootProject.Projects.Project.Add(project1);
            _projects.Add(project1);
        }

        public ProjectDto ById(string id) => _projects.SingleOrDefault(u => u.Id == id);
        public ProjectsDto All() => new ProjectsDto { Project = _projects };
    }
}