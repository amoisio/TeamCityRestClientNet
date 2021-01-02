using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class VcsRootListDto : ListDto<VcsRootDto>
    {
        [JsonProperty(PropertyName = "vcs-root")]
        public override List<VcsRootDto> Items { get; set; } = new List<VcsRootDto>();
    }

    public class VcsRootDto : IdDto
    {
        public string Name { get; set; }
        public NameValuePropertiesDto Properties { get; set; }
    }

    public class VcsRootInstanceDto
    {
        public string VcsRootId { get; set; }
        public string Name { get; set; }
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

}