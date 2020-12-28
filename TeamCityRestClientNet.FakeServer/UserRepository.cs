using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class UserRepository 
    {
        public static readonly UserDto JohnDoe = new UserDto 
        { 
            Id = "1", 
            Name = "John Doe", 
            Username = "jodoe", 
            Email = "john.doe@mailinator.com" 
        };

        public static readonly UserDto JaneDoe = new UserDto 
        { 
            Id = "2", 
            Name = "Jane Doe", 
            Username = "jadoe", 
            Email = "jane.doe@mailinator.com" 
        };

        public static readonly UserDto DunkinDonuts = new UserDto 
        { 
            Id = "3", 
            Name = "Dunkin' Donuts", 
            Username = "dunkin", 
            Email = "dunkin@mailinator.com" 
        };

        public static readonly UserDto MacCheese = new UserDto 
        { 
            Id = "4", 
            Name = "Mac Cheese", 
            Username = "maccheese", 
            Email = "maccheese@mailinator.com" 
        };

        private static readonly List<UserDto> _users = new List<UserDto>
        {
            JohnDoe,
            JaneDoe,
            DunkinDonuts,
            MacCheese
        };

        public UserDto ById(string id) => _users.SingleOrDefault(u => u.Id == id);
        public UserDto ByUsername(string username) => _users.SingleOrDefault(u => u.Username == username);
        public UserListDto All() => new UserListDto { User = _users };
    }
}