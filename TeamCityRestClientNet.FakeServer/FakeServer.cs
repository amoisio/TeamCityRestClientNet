using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    /// <summary>
    /// In-memory team city server for faking responses.
    /// </summary>
    public class FakeServer
    {
        private readonly ILogger _logger;
        private readonly DataBuilder _data;
        public FakeServer(ILogger logger)
        {
            _logger = logger;
            _data = new DataBuilder();
            _data.Load();
        }

        internal BuildAgentRepository BuildAgents => _data.BuildAgents;
        internal BuildAgentPoolRepository BuildAgentPools => _data.BuildAgentPools;
        internal BuildTypeRepository BuildTypes => _data.BuildTypes;
        internal ChangeRepository Changes => _data.Changes;
        internal ProjectRepository Projects => _data.Projects;
        internal UserRepository Users => _data.Users;
        internal VcsRootRepository VcsRoots => _data.VcsRoots;
        internal VcsRootInstanceRepository VcsRootInstances => _data.VcsRootInstances;

        public object ResolveApiCall(ApiCall apiCall)
        {
            object response = apiCall.Resource switch
            {
                "agents" => ResolveAgents(apiCall),
                "agentPools" => Resolve(apiCall, BuildAgentPools),
                "buildTypes" => Resolve(apiCall, BuildTypes),
                "changes" => Resolve(apiCall, Changes),
                "projects" => ResolveProjects(apiCall),
                "users" => ResolveUsers(apiCall),
                "vcs-roots" => ResolveVcsRoots(apiCall),
                "vcs-root-instances" => Resolve(apiCall, VcsRootInstances),
                _ => throw new NotImplementedException()
            };
            return response;
        }

        private object Resolve<TDto, TListDto>(ApiCall apiCall, BaseRepository<TDto, TListDto> repository)
            where TDto : IdDto
            where TListDto : ListDto<TDto>, new()
        {
            if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators)
                    return repository.All();
                else
                    return repository.ById(apiCall.GetLocatorValue());
            }
            else if (apiCall.Method == HttpMethod.Delete)
            {
                return repository.Delete(apiCall.GetLocatorValue());
            }
            else
            {
                throw new NotSupportedException($"Method {apiCall.Method} not supported by generic Resolve.");
            }
        }

        private object ResolveAgents(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Put)
            {
                var id = apiCall.GetLocatorValue();
                if (apiCall.Content == "false")
                {
                    return BuildAgents.Disable(id);
                } 
                else if (apiCall.Content == "true")
                {
                    return BuildAgents.Enable(id);
                }
                else
                    throw new NotImplementedException();
            }
            else
            {
                return Resolve<BuildAgentDto, BuildAgentListDto>(apiCall, BuildAgents);
            }
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
    }
}
