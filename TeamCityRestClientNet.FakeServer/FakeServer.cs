using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class FakeServer
    {
        public FakeServer()
        {
            Users = new UserRepository();
        }

        public UserRepository Users { get; }

        public object ResolveApiCall(ApiCall apiCall)
        {
            object response = apiCall.Resource switch
            {
                "users" => ResolveUsers(apiCall),
                _       => throw new NotImplementedException()
            };
            return response;
        }

        private object ResolveUsers(ApiCall apiCall)
        {
            if (!apiCall.HasLocators)
                return this.Users.All();
            else if (apiCall.Locators.ContainsKey("id"))
                return this.Users.ById(apiCall.Locators["id"]);
            else
                return this.Users.ByUsername(apiCall.Locators["username"]);
        }

        
    }
}
