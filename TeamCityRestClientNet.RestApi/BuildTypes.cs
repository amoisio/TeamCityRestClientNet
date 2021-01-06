using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildTypeListDto : ListDto<BuildTypeDto>
    {
        [JsonProperty(PropertyName = "buildType")]
        public override List<BuildTypeDto> Items { get; set; } = new List<BuildTypeDto>();
    }

    public class BuildTypeDto : IdDto
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public bool? Paused { get; set; }
        public BuildTypeSettingsDto Settings { get; set; }
    }

    public class BuildTypeSettingsDto
    {
        public List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    public class TriggersDto
    {
        public List<TriggerDto> Trigger { get; set; } = new List<TriggerDto>();
    }

    public class TriggerDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public ParametersDto Properties { get; set; } = new ParametersDto();
    }

    public class ArtifactDependenciesDto
    {
        public List<ArtifactDependencyDto> ArtifactDependency { get; set; } = new List<ArtifactDependencyDto>();
    }

    public class ArtifactDependencyDto : IdDto
    {
        public string Type { get; set; }
        public bool? Disabled { get; set; } = false;
        public bool? Inherited { get; set; } = false;
        public ParametersDto Properties { get; set; } = new ParametersDto();
        public BuildTypeDto SourceBuildType { get; set; } = new BuildTypeDto();
    }


    [XmlRoot("newBuildTypeDescription")]
    public class NewBuildTypeDescription
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("sourceBuildTypeLocator")]
        public string SourceBuildTypeLocator { get; set; }
        [XmlElement("copyAllAssociatedSettings")]
        public bool CopyAllAssociatedSettings { get; set; }
        [XmlElement("shareVCSRoots")]
        public bool ShareVCSRoots { get; set; }
    }
}