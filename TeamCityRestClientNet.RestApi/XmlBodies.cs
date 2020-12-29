using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TeamCityRestClientNet.RestApi
{
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

    [XmlRoot("vcs-root")]
    public class NewVcsRoot 
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("vcsName")]
        public string VcsName { get; set; }
        [XmlElement("project")]
        public ReferenceDto Project { get; set; }
        [XmlElement("properties")]
        public PropertiesDto Properties { get; set; }

        public VcsRootDto ToVcsRootDto()
        {
            return new VcsRootDto
            {
                Id = this.Id,
                Name = this.Name,
                Properties = new NameValuePropertiesDto
                {
                    Property = this.Properties.Property.Select(prop => 
                    new NameValuePropertyDto
                    {
                        Name = prop.Name,
                        Value = prop.Value
                    }).ToList() ?? new List<NameValuePropertyDto>()
                }
            };
        }
    }

    public class ReferenceDto
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class PropertiesDto
    {
        [XmlElement("property")]
        public List<PropertyDto> Property { get; set; }
    }

    public class PropertyDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}