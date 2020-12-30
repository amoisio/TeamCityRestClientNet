using System;
using System.Net.Http;

namespace TeamCityRestClientNet.FakeServer
{
    /// <summary>
    /// In-memory team city server for faking responses.
    /// </summary>
    class FakeServer
    {
        public FakeServer()
        {
            Users = new UserRepository();
            VcsRoots = new VcsRootRepository();
            Projects = new ProjectRepository();
            BuildTypes = new BuildTypeRepository();
        }

        public UserRepository Users { get; }
        public VcsRootRepository VcsRoots { get; }
        public ProjectRepository Projects { get; }
        public BuildTypeRepository BuildTypes { get; }

        public object ResolveApiCall(ApiCall apiCall)
        {
            object response = apiCall.Resource switch
            {
                "users" => ResolveUsers(apiCall),
                "vcs-roots" => ResolveVcsRoots(apiCall),
                "projects" => ResolveProjects(apiCall),
                "buildTypes" => ResolveBuildTypes(apiCall),
                _ => throw new NotImplementedException()
            };
            return response;
        }

        private object ResolveUsers(ApiCall apiCall)
        {
            if (!apiCall.HasLocators)
                return this.Users.All();
            else if (apiCall.HasNamedLocator("username"))
                return this.Users.ByUsername(apiCall.GetLocatorValue("username"));
            else
                return this.Users.ById(apiCall.GetLocatorValue());
        }

        private object ResolveVcsRoots(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators)
                    return this.VcsRoots.All();
                else
                    return this.VcsRoots.ById(apiCall.GetLocatorValue());
            }
            else if (apiCall.Method == HttpMethod.Post)
            {
                var xmlString = apiCall.Request.Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                return this.VcsRoots.Create(xmlString);
            } 
            else if (apiCall.Method == HttpMethod.Delete)
            {
                return this.VcsRoots.Delete(apiCall.GetLocatorValue());
            }
            else 
            {
                throw new NotImplementedException();
            }
        }

        private object ResolveProjects(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators)
                    return this.Projects.All();
                else
                    return this.Projects.ById(apiCall.GetLocatorValue());
            } 
            else if (apiCall.Method == HttpMethod.Post)
            {
                var xmlString = apiCall.Request.Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                return this.Projects.Create(xmlString);
            }
            else if (apiCall.Method == HttpMethod.Delete)
            {
                return this.Projects.Delete(apiCall.GetLocatorValue());
            } 
            else if (apiCall.Method == HttpMethod.Put)
            {
                if (apiCall.Property == "parameters")
                {
                    var id = apiCall.GetLocatorValue();
                    var name = apiCall.Descriptor;
                    var value = apiCall.Content;
                    return this.Projects.SetParameter(id, name, value);
                } 
            }
            throw new NotImplementedException();
        }

        private object ResolveBuildTypes(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators)
                    return this.BuildTypes.All();
                else
                    return this.BuildTypes.ById(apiCall.GetLocatorValue());
            }
            else
            {
                throw new NotImplementedException();
            }
        }


    }
}
