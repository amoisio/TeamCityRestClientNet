using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildProblemOccurrenceListDto : ListDto<BuildProblemOccurrenceDto>
    {
        [JsonProperty(PropertyName = "problemOccurrence")]
        public override List<BuildProblemOccurrenceDto> Items { get; set; } = new List<BuildProblemOccurrenceDto>();
    }

    public class BuildProblemOccurrenceDto : IdDto
    {
        public string Type { get; set; }
        public string Identity { get; set; }
        public string Details { get; set; }
        public string AdditionalData { get; set; }
        public BuildProblemDto Problem { get; set; }
        public BuildDto Build { get; set; }
    }

    public class BuildProblemDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Identity { get; set; }
    }
}