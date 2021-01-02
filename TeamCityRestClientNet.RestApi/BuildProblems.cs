using System.Collections.Generic;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildProblemOccurrencesDto
    {
        public string NextHref { get; set; }
        public List<BuildProblemOccurrenceDto> ProblemOccurrence { get; set; } = new List<BuildProblemOccurrenceDto>();
    }

    public class BuildProblemOccurrenceDto
    {
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