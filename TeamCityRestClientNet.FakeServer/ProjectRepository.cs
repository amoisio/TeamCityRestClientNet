using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class ProjectRepository
    {
        public static ProjectDto RootProject = new ProjectDto
        {
            Id = "_Root",
            Name = "<Root project>"
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

        private static readonly Dictionary<string, ProjectDto> _projects;

        static ProjectRepository()
        {
            RootProject.Projects.Project.Add(RestClientProject);
            RootProject.Projects.Project.Add(TeamCityCliProject);
            var project1 = new ProjectDto
            {
                Id = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4",
                ParentProjectId = "_Root",
                Name = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4"
            };
            RootProject.Projects.Project.Add(project1);

            _projects = new Dictionary<string, ProjectDto>();
            _projects.Add(RootProject.Id, RootProject);
            _projects.Add(RestClientProject.Id, RestClientProject);
            _projects.Add(TeamCityCliProject.Id, TeamCityCliProject);
            _projects.Add(project1.Id, project1);
        }

        public ProjectDto ById(string id) => _projects[id];
        public ProjectsDto All() => new ProjectsDto { Project = _projects.Values.ToList() };
        public ProjectDto Create(string xmlString)
        {
            using (var strReader = new StringReader(xmlString))
            using (var xmlReader = XmlReader.Create(strReader))
            {
                var serializer = new XmlSerializer(typeof(NewProjectDescriptionDto));
                var newDto = serializer.Deserialize(xmlReader) as NewProjectDescriptionDto;

                // TODO: Refactor id checks elsewhere. TeamCity has a limited set of characters which are suitable
                // for Ids. - is not one of those characters.
                if (newDto.Id.Contains('-'))
                {
                    throw new InvalidOperationException("Invalid character in id.");
                }

                var dto = newDto.ToProjectDto();

                var parent = ById(dto.ParentProjectId);
                parent.Projects.Project.Add(dto);
                _projects.Add(dto.Id, dto);
                return dto;
            }
        }

        public ProjectDto Delete(string id)
        {
            var toDelete = ById(id);
            var parent = ById(toDelete.ParentProjectId);
            _projects.Remove(id);
            parent.Projects.Project.Remove(toDelete);
            return toDelete;
        }

        public ProjectDto SetParameter(string id, string name, string value)
        {
            var project = ById(id);
            var param = project.Parameters.Property.SingleOrDefault(p => p.Name == name);
            if (param == null)
            {
                project.Parameters.Property.Add(new ParameterDto(name, value));
            } 
            else 
            {
                param.Value = value;
            }
            return project;
        }
    }
}