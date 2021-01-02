using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class ProjectsDto : ListDto<ProjectDto>
    {
        [JsonProperty(PropertyName = "project")]
        public override List<ProjectDto> Items { get; set; }
    }

    public class ProjectDto : IdDto
    {
        public string Name { get; set; }
        public string ParentProjectId { get; set; }
        public bool? Archived { get; set; }
        public ProjectsDto Projects { get; set; } = new ProjectsDto();
        public ParametersDto Parameters { get; set; } = new ParametersDto();
        public BuildTypesDto BuildTypes { get; set; } = new BuildTypesDto();
    }
}