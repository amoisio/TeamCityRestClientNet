using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class ProjectListDto : ListDto<ProjectDto>
    {
        [JsonProperty(PropertyName = "project")]
        public override List<ProjectDto> Items { get; set; } = new List<ProjectDto>();
    }

    public class ProjectDto : IdDto
    {
        public string Name { get; set; }
        public string ParentProjectId { get; set; }
        public bool? Archived { get; set; }
        public ProjectListDto Projects { get; set; } = new ProjectListDto();
        public ParametersDto Parameters { get; set; } = new ParametersDto();
        public BuildTypeListDto BuildTypes { get; set; } = new BuildTypeListDto();
    }

    [XmlRoot("newProjectDescription")]
    public class NewProjectDescriptionDto
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("copyAllAssociatedSettings")]
        public bool CopyAllAssociatedSettings { get; set; }
        [XmlElement("parentProject")]
        public ProjectLocatorDto ParentProject { get; set; }
        [XmlElement("sourceProject")]
        public ProjectLocatorDto SourceProject { get; set; }

        public ProjectDto ToProjectDto()
        {
            return new ProjectDto
            {
                Id = this.Id,
                Name = this.Name,
                ParentProjectId = this.ParentProject.Locator.Split(':')[1].Trim()
            };
        }
    }

    public class ProjectLocatorDto
    {
        [XmlAttribute("locator")]
        public string Locator { get; set; }
    }
}