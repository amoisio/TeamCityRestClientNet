using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class ProjectRepository : BaseRepository<ProjectDto, ProjectsDto>
    {
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