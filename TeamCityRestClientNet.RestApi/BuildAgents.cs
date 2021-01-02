using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildAgentsDto : ListDto<BuildAgentDto>
    {
        [JsonProperty(PropertyName = "agent")]
        public override List<BuildAgentDto> Items { get; set; }
    }

    public class BuildAgentDto : IdDto
    {
        public string Name { get; set; }
        public bool? Connected { get; set; }
        public bool? Enabled { get; set; }
        public bool? Authorized { get; set; }
        public bool? Uptodate { get; set; }
        public string Ip { get; set; }
        public EnabledInfoDto EnabledInfo { get; set; }
        public AuthorizedInfoDto AuthorizedInfo { get; set; }
        public ParametersDto Properties { get; set; } = new ParametersDto();
        public BuildAgentPoolDto Pool { get; set; }
        public BuildDto Build { get; set; }
    }
}