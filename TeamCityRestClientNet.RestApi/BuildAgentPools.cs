using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildAgentPoolsDto : ListDto<BuildAgentPoolDto>
    {
        [JsonProperty(PropertyName = "agentPool")]
        public override List<BuildAgentPoolDto> Items { get; set; }
    }
    
    public class BuildAgentPoolDto : IdDto
    {
        public string Name { get; set; }
        public ProjectsDto Projects { get; set; } = new ProjectsDto();
        public BuildAgentsDto Agents { get; set; } = new BuildAgentsDto();
    }
}