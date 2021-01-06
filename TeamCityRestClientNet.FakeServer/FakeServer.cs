using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using TeamCityRestClientNet.RestApi;
using Newtonsoft.Json;

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
            BuildQueue = new BuildQueue(_data.Builds);
        }

        internal BuildRepository Builds => _data.Builds;
        internal BuildAgentRepository BuildAgents => _data.BuildAgents;
        internal BuildAgentPoolRepository BuildAgentPools => _data.BuildAgentPools;
        internal BuildQueue BuildQueue { get; }
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
                "agents"             => ResolveAgents(apiCall),
                "agentPools"         => ResolveAgentPools(apiCall),
                "builds"             => ResolveBuilds(apiCall),
                "buildQueue"         => ResolveBuildQueue(apiCall),
                "buildTypes"         => ResolveBuildTypes(apiCall),
                "changes"            => ResolveChanges(apiCall),
                "investigations"     => ResolveInvestigations(apiCall),
                "problemOccurrences" => ResolveProblems(apiCall),
                "projects"           => ResolveProjects(apiCall),
                "testOccurrences"    => ResolveTests(apiCall),
                "users"              => ResolveUsers(apiCall),
                "vcs-roots"          => ResolveVcsRoots(apiCall),
                "vcs-root-instances" => ResolveVcsRootInstances(apiCall),
                _ => throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.")
            };
            return response;
        }

        private object ResolveAgents(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Put)
            {
                // [Headers("Accept: text/plain", "Content-Type: text/plain")]
                // [Put("/app/rest/agents/{locator}/enabled")]
                // Task EnableAgent([AliasAs("locator")] string agentlocator, [Body(BodySerializationMethod.Serialized)] bool enable);
                var id = apiCall.GetLocatorValue();
                if (apiCall.Content == "false")
                {
                    return BuildAgents.Disable(id);
                }
                else if (apiCall.Content == "true")
                {
                    return BuildAgents.Enable(id);
                }
            }
            else
            {
                // [Headers("Accept: application/json")]
                // [Get("/app/rest/agents")]
                // Task<BuildAgentListDto> Agents();

                // [Headers("Accept: application/json")]
                // [Get("/app/rest/agents/{locator}")]
                // Task<BuildAgentDto> Agent([AliasAs("locator")] string agentlocator);
                return Resolve<BuildAgentDto, BuildAgentListDto>(apiCall, BuildAgents);
            }

            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveAgentPools(ApiCall apiCall)
        {
            // [Headers("Accept: application/json")]
            // [Get("/app/rest/agentPools")]
            // Task<BuildAgentPoolListDto> AgentPools();

            // [Headers("Accept: application/json")]
            // [Get("/app/rest/agentPools/{locator}")]
            // Task<BuildAgentPoolDto> AgentPool([AliasAs("locator")] string agentPoolLocator);
            return Resolve(apiCall, BuildAgentPools);
        }
        private object ResolveBuilds(ApiCall apiCall)
        {
            var id = apiCall.GetLocatorOrDefault();

            if (apiCall.Method == HttpMethod.Post)
            {
                if (String.IsNullOrEmpty(apiCall.Property))
                {
                    // [Headers("Accept: application/json")]
                    // [Post("/app/rest/builds/{id}")]
                    // Task CancelBuild([AliasAs("id")] string buildId, [Body] BuildCancelRequestDto value);
                    Builds.CancelBuild(id);
                    return id;
                }
                else
                {
                    // [Headers("Accept: application/json")]
                    // [Post("/app/rest/builds/{id}/tags/")]
                    // Task AddTag([AliasAs("id")] string buildId, [Body] string tag);
                    Builds.AddTag(id, apiCall.Content);
                    return id;
                }
            }
            else if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators)
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/builds")]
                    // Task<BuildListDto> Builds([AliasAs("locator")] string buildLocator);
                    return Builds.All();
                }

                if (String.IsNullOrEmpty(apiCall.Property))
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/builds/{id}")]
                    // Task<BuildDto> Build(string id);
                    return Builds.ById(apiCall.GetLocatorValue());
                }
                else if (apiCall.Property == "resulting-properties")
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/builds/{id}/resulting-properties")]
                    // Task<ParametersDto> ResultingProperties([AliasAs("id")] string buildId);
                    return Builds.ResultingProperties(id);
                }
                else if (apiCall.Property == "artifacts" && apiCall.Descriptor == "children")
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/builds/{id}/artifacts/children/{**path}")]
                    // Task<ArtifactFileListDto> ArtifactChildren([AliasAs("id")] string buildId, [AliasAs("path")] string artifactPath, string locator, string fields);
                    return Builds.Artifacts(id);
                }
                else if (apiCall.Property == "artifacts" && apiCall.Descriptor == "content")
                {
                    // [Get("/app/rest/builds/{id}/artifacts/content/{**path}")]
                    // Task<Stream> ArtifactContent( [AliasAs("id")] string buildId, [AliasAs("path")] string artifactPath);
                    return Builds.Content(id);
                }
            }
            else if (apiCall.Method == HttpMethod.Put)
            {
                if (apiCall.Property == "comment")
                {
                    // [Put("/app/rest/builds/{id}/comment/")]
                    // Task SetComment([AliasAs("id")] string buildId, [Body] string comment);
                    Builds.SetComment(id, apiCall.Content);
                    return id;
                }
                else if (apiCall.Property == "tags")
                {
                    // [Put("/app/rest/builds/{id}/tags/")]
                    // Task ReplaceTags([AliasAs("id")] string buildId, [Body] TagsDto tags);
                    Builds.ReplaceTags(id, apiCall.JsonContentAs<TagsDto>());
                    return id;
                }
                else if (apiCall.Property == "pin")
                {
                    // [Put("/app/rest/builds/{id}/pin/")]
                    // Task Pin([AliasAs("id")] string buildId, [Body] string comment);
                    Builds.Pin(id, apiCall.Content);
                    return id;
                }
            }
            else if (apiCall.Method == HttpMethod.Delete)
            {
                // //The standard DELETE annotation doesn't allow to include a body, so we need to use our own.
                // //Probably it would be better to change Rest API here (https://youtrack.jetbrains.com/issue/TW-49178).
                // // @DELETE_WITH_BODY("/app/rest/builds/{id}/pin/")
                // [Delete("/app/rest/builds/{id}/pin/")]
                // Task Unpin([AliasAs("id")] string buildId, [Body] string comment);
                Builds.Unpin(id);
                return id;
            }

            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveBuildQueue(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Post)
            {
                if (!apiCall.HasLocators) 
                {
                    // [Headers("Accept: application/json")]
                    // [Post("/app/rest/buildQueue")]
                    // Task<TriggeredBuildDto> TriggerBuild([Body] TriggerBuildRequestDto value);
                    var request = JsonConvert.DeserializeObject<TriggerBuildRequestDto>(apiCall.Content);
                    return BuildQueue.TriggerBuild(request);
                }
                else 
                {
                    // [Headers("Accept: application/json")]
                    // [Post("/app/rest/buildQueue/{id}")]
                    // Task RemoveQueuedBuild([AliasAs("id")] string buildId, [Body] BuildCancelRequestDto value);
                    var request = JsonConvert.DeserializeObject<BuildCancelRequestDto>(apiCall.Content);
                    var id = apiCall.GetLocatorValue();
                    BuildQueue.CancelBuild(id, request);
                    return id;
                }
            }
            else if (apiCall.Method == HttpMethod.Get)
            {
                // [Headers("Accept: application/json")]
                // [Get("/app/rest/buildQueue")]
                // Task<BuildListDto> QueuedBuilds(string locator);
                return new BuildListDto
                {
                    Items = BuildQueue
                };
            }
            
            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveBuildTypes(ApiCall apiCall)
        {
            var id = apiCall.GetLocatorOrDefault();

            if (apiCall.Method == HttpMethod.Post)
            {
                // [Headers("Accept: application/json", "Content-Type: application/xml")]
                // [Post("/app/rest/buildTypes")]
                // Task<BuildTypeDto> CreateBuildType([Body] string buildTypeXml);
                return BuildTypes.Create(apiCall.Content);
            }
            else if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators)
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/buildTypes")]
                    // Task<BuildTypeListDto> BuildTypes();
                    return BuildTypes.All();
                }
                if (String.IsNullOrEmpty(apiCall.Property))
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/buildTypes/{id}")]
                    // Task<BuildTypeDto> BuildType([AliasAs("id")] string buildTypeId);
                    return BuildTypes.ById(id);
                }
                else if (apiCall.Property == "buildTags")
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/buildTypes/{id}/buildTags")]
                    // Task<TagsDto> BuildTypeTags([AliasAs("id")] string buildTypeId);
                    return BuildTypes.Tags(id);
                }
                else if (apiCall.Property == "triggers")
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/buildTypes/{id}/triggers")]
                    // Task<TriggersDto> BuildTypeTriggers([AliasAs("id")] string buildTypeId);
                    return BuildTypes.Triggers(id);
                }
                else if (apiCall.Property == "artifact-dependencies")
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/buildTypes/{id}/artifact-dependencies")]
                    // Task<ArtifactDependenciesDto> BuildTypeArtifactDependencies([AliasAs("id")] string buildTypeId);
                    return BuildTypes.ArtifactDependencies(id);
                }
            }
            else if (apiCall.Method == HttpMethod.Put)
            {
                if (apiCall.Property == "parameters") 
                {
                    // [Put("/app/rest/buildTypes/{id}/parameters/{name}")]
                    // Task SetBuildTypeParameter([AliasAs("id")] string buildTypeId, string name, [Body] string value);
                    BuildTypes.SetParameters(id, apiCall.Descriptor, apiCall.Content);
                    return id;
                } 
                else if (apiCall.Property == "settings")
                {
                    // [Put("/app/rest/buildTypes/{id}/settings/{name}")]
                    // Task SetBuildTypeSettings([AliasAs("id")] string buildTypeId, string name, [Body] string value);
                    BuildTypes.SetSetting(id, apiCall.Descriptor, apiCall.Content);
                    return id;
                }
            }
            else if (apiCall.Method == HttpMethod.Delete)
            {
                BuildTypes.Delete(id);
                return id;
            }

            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveChanges(ApiCall apiCall)
        {
            // [Headers("Accept: application/json")]
            // [Get("/app/rest/changes/{id},{version}")]
            // Task<ChangeDto> Change([AliasAs("id")] string buildType, [AliasAs("version")] string version);

            // [Headers("Accept: application/json")]
            // [Get("/app/rest/changes/{id}")]
            // Task<ChangeDto> Change([AliasAs("id")] string changeId);

            // [Headers("Accept: application/json")]
            // [Get("/app/rest/changes")]
            // Task<ChangeListDto> Changes(string locator, string fields);
            return Resolve<ChangeDto, ChangeListDto>(apiCall, Changes);
        }
        private object ResolveInvestigations(ApiCall apiCall)
        {
            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveProblems(ApiCall apiCall)
        {
            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveProjects(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Post)
            {
                // [Headers("Accept: application/json", "Content-Type: application/xml")]
                // [Post("/app/rest/projects")]
                // Task<ProjectDto> CreateProject([Body] string projectDescriptionXml);
                return this.Projects.Create(apiCall.Content);
            }
            else if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators) 
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/projects")]
                    // Task<ProjectListDto> Projects();
                    return this.Projects.All();
                }
                else 
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/projects/{id}")]
                    // Task<ProjectDto> Project(string id);
                    return this.Projects.ById(apiCall.GetLocatorValue());
                }
            }
            else if (apiCall.Method == HttpMethod.Put)
            {
                if (apiCall.Property == "parameters")
                {
                    // [Put("/app/rest/projects/{id}/parameters/{name}")]
                    // Task SetProjectParameter([AliasAs("id")] string projectId, string name, [Body] string value);
                    var id = apiCall.GetLocatorValue();
                    var name = apiCall.Descriptor;
                    var value = apiCall.Content;
                    return this.Projects.SetParameter(id, name, value);
                }
            }
            else if (apiCall.Method == HttpMethod.Delete)
            {
                // [Headers("Accept: application/json")]
                // [Delete("/app/rest/projects/{locator}")]
                // Task DeleteProject([AliasAs("locator")] string projectLocator);
                return this.Projects.Delete(apiCall.GetLocatorValue());
            }

            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveTests(ApiCall apiCall)
        {
            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveUsers(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators) 
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/users")]
                    // Task<UserListDto> Users();
                    return this.Users.All();
                }
                else if (apiCall.HasNamedLocator("username")) 
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/users/{locator}")]
                    // Task<UserDto> Users([AliasAs("locator")] string userLocator);
                    return this.Users.ByUsername(apiCall.GetLocatorValue("username"));
                }
                else
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/users/{locator}")]
                    // Task<UserDto> Users([AliasAs("locator")] string userLocator);
                    return this.Users.ById(apiCall.GetLocatorValue());
                }
            }

            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveVcsRoots(ApiCall apiCall)
        {
            if (apiCall.Method == HttpMethod.Post)
            {
                // [Headers("Accept: application/json", "Content-Type: application/xml")]
                // [Post("/app/rest/vcs-roots")]
                // Task<VcsRootDto> CreateVcsRoot([Body] string vcsRootXml);
                return this.VcsRoots.Create(apiCall.Content);
            }
            else if (apiCall.Method == HttpMethod.Get)
            {
                if (!apiCall.HasLocators) 
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/vcs-roots")]
                    // Task<VcsRootListDto> VcsRoots();
                    return this.VcsRoots.All();
                }
                else 
                {
                    // [Headers("Accept: application/json")]
                    // [Get("/app/rest/vcs-roots/{id}")]
                    // Task<VcsRootDto> VcsRoot(string id);
                    return this.VcsRoots.ById(apiCall.GetLocatorValue());
                }
            }
            else if (apiCall.Method == HttpMethod.Delete)
            {
                // [Headers("Accept: application/json")]
                // [Delete("/app/rest/vcs-roots/{locator}")]
                // Task DeleteVcsRoot([AliasAs("locator")] string vcsRootLocator);
                return this.VcsRoots.Delete(apiCall.GetLocatorValue());
            }

            throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
        }
        private object ResolveVcsRootInstances(ApiCall apiCall)
        {
            // [Headers("Accept: application/json")]
            // [Get("/app/rest/vcs-root-instances")]
            // Task<VcsRootInstanceListDto> VcsRootInstances();

            // [Headers("Accept: application/json")]
            // [Get("/app/rest/vcs-root-instances/{id}")]
            // Task<VcsRootInstanceDto> VcsRootInstance(string id);
            return Resolve<VcsRootInstanceDto, VcsRootInstanceListDto>(apiCall, VcsRootInstances);
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
                throw new NotImplementedException($"End-point {apiCall.Method} : {apiCall.RequestPath} not implemented.");
            }
        }
    }
}
