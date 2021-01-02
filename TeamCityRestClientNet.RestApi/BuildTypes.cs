using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildTypesDto : ListDto<BuildTypeDto>
    {
        [JsonProperty(PropertyName = "buildType")]
        public override List<BuildTypeDto> Items { get; set; }
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
}