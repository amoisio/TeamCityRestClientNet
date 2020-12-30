using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class ProjectRepository : BaseRepository<ProjectDto>
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
            }
        };

        public static ProjectDto TeamCityCliProject = new ProjectDto
        {
            Id = "TeamCityCliNet",
            ParentProjectId = "_Root",
            Name = "TeamCity CLI .NET"
        };

        public static ProjectDto Project1 = new ProjectDto
        {
            Id = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4",
            ParentProjectId = "_Root",
            Name = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4"
        };

        static ProjectRepository() { }
        public ProjectRepository()
            : base (project => project.Id, RootProject, RestClientProject, TeamCityCliProject, Project1) 
        {
            RootProject.Projects.Project.Add(RestClientProject);
            TeamCityCliProject.BuildTypes.BuildType.Add(BuildTypeRepository.RestClient);
            RootProject.Projects.Project.Add(TeamCityCliProject);
            TeamCityCliProject.BuildTypes.BuildType.Add(BuildTypeRepository.TeamCityCli);
            RootProject.Projects.Project.Add(Project1);
        }

        public ProjectsDto All() => new ProjectsDto { Project = AllItems() };
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
                _itemsById.Add(dto.Id, dto);
                return dto;
            }
        }

        public override ProjectDto Delete(string id)
        {
            var toDelete = base.Delete(id);
            var parent = ById(toDelete.ParentProjectId);
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