using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class VcsRootListDto : ListDto<VcsRootDto>
    {
        [JsonProperty(PropertyName = "vcs-root")]
        public override List<VcsRootDto> Items { get; set;}
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

}