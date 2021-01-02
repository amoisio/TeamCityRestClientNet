using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCityRestClientNet.RestApi
{
    public class UserListDto : ListDto<UserDto>
    {
        [JsonProperty(PropertyName = "user")]
        public override List<UserDto> Items { get; set; } = new List<UserDto>();
    }

    public class UserDto : IdDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}