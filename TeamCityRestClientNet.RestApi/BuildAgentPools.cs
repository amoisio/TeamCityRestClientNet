using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildAgentPoolListDto : ListDto<BuildAgentPoolDto>
    {
        [JsonProperty(PropertyName = "agentPool")]
        public override List<BuildAgentPoolDto> Items { get; set; }
    }
    
    public class BuildAgentPoolDto : IdDto
    {
        public string Name { get; set; }
        public ProjectListDto Projects { get; set; } = new ProjectListDto();
        public BuildAgentListDto Agents { get; set; } = new BuildAgentListDto();
    }
}