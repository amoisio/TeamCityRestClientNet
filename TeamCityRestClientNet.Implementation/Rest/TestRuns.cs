using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class TestOccurrenceListDto : ListDto<TestOccurrenceDto>
    {
        [JsonProperty(PropertyName = "testOccurrence")]
        public override List<TestOccurrenceDto> Items { get; set; } = new List<TestOccurrenceDto>();
    }

    public class TestOccurrenceDto : IdDto
    {
        public string Status { get; set; }
        public bool? Ignored { get; set; }
        public long? Duration { get; set; }
        public string IgnoreDetails { get; set; }
        public string Details { get; set; }
        public bool? CurrentlyMuted { get; set; }
        public bool? Muted { get; set; }
        public bool? NewFailure { get; set; }
        public BuildDto Build { get; set; }
        public TestDto Test { get; set; }
        public BuildDto NextFixed { get; set; }
        public BuildDto FirstFailed { get; set; }
        public const string FILTER = "testOccurrence(name,status,ignored,muted,currentlyMuted,newFailure,duration,ignoreDetails,details,firstFailed(id),nextFixed(id),build(id),test(id))";
    }

    public class TestDto : IdDto
    {
        
    }
}