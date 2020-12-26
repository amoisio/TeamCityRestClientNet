using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class FakeServer
    {
        public object ResolveApiCall(ApiCall apiCall)
        {
            if (apiCall.Resource == "users")
            {
                if (!apiCall.HasLocators)
                    return this.Users();
                else if (apiCall.Locators.ContainsKey("id"))
                    return this.UserById(apiCall.Locators["id"]);
                else
                    return this.UserByUsername(apiCall.Locators["username"]);
            }
            throw new NotImplementedException();
        }

        private List<UserDto> _users = new List<UserDto>
        {
            new UserDto { Id = "1", Name = "John Doe", Username = "jodoe", Email = "john.doe@mailinator.com" },
            new UserDto { Id = "2", Name = "Jane Doe", Username = "jadoe", Email = "jane.doe@mailinator.com" },
            new UserDto { Id = "3", Name = "Dunkin' Donuts", Username = "dunkin", Email = "dunkin@mailinator.com" },
            new UserDto { Id = "4", Name = "Mac Cheese", Username = "maccheese", Email = "maccheese@mailinator.com" }
        };
        private UserDto UserById(string id) => _users.SingleOrDefault(u => u.Id == id);
        private UserDto UserByUsername(string username) => _users.SingleOrDefault(u => u.Username == username);
        private UserListDto Users() => new UserListDto { User = _users };
    }
}
