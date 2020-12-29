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
}