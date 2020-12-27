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
            VcsRoots = new VcsRootRepository();
        }

        public UserRepository Users { get; }
        public VcsRootRepository VcsRoots { get; }

        public object ResolveApiCall(ApiCall apiCall)
        {
            object response = apiCall.Resource switch
            {
                "users"     => ResolveUsers(apiCall),
                "vcs-roots" => ResolveVcsRoots(apiCall),
                _           => throw new NotImplementedException()
            };
            return response;
        }

        private object ResolveVcsRoots(ApiCall apiCall)
        {
            if (!apiCall.HasLocators)
                return this.VcsRoots.All();
            else 
                return this.VcsRoots.ById(apiCall.GetLocatorValue("id"));
        }

        private object ResolveUsers(ApiCall apiCall)
        {
            if (!apiCall.HasLocators)
                return this.Users.All();
            else if (apiCall.Locators.ContainsKey("username"))
                return this.Users.ByUsername(apiCall.Locators["username"]);
            else
                return this.Users.ById(apiCall.GetLocatorValue("id"));
        }
    }
}
