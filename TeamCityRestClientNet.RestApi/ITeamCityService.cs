using System;
using System.IO;
using System.Threading.Tasks;
using Refit;

namespace TeamCityRestClientNet.RestApi
{
    public interface ITeamCityService
    {
        [Headers("Accept: application/json")]
        [Get("/{**path}")]
        [QueryUriFormat(UriFormat.Unescaped)]
        Task<T> Root<T>(string path);

        [Headers("Accept: application/json")]
        [Get("/app/rest/builds")]
        Task<BuildListDto> Builds([AliasAs("locator")] string buildLocator);

        [Headers("Accept: application/json")]
        [Get("/app/rest/buildQueue")]
        Task<BuildListDto> QueuedBuilds(string locator);

        [Headers("Accept: application/json")]
        [Get("/app/rest/builds/{id}")]
        Task<BuildDto> Build(string id);

        [Headers("Accept: application/json")]
        [Get("/app/rest/investigations")]
        Task<InvestigationListDto> Investigations([AliasAs("locator")] string investigationLocator);

        [Headers("Accept: application/json")]
        [Get("/app/rest/investigations/{id}")]
        Task<InvestigationDto> Investigation(string id);

        

        [Headers("Accept: application/json")]
        [Get("/app/rest/testOccurrences/")]
        Task<TestOccurrencesDto> TestOccurrences(string locator, string fields);

        [Headers("Accept: application/json")]
        [Get("/app/rest/vcs-roots")]
        Task<VcsRootListDto> VcsRoots();

        [Headers("Accept: application/json")]
        [Get("/app/rest/vcs-roots/{id}")]
        Task<VcsRootDto> VcsRoot(string id);

        [Post("/app/rest/builds/{id}/tags/")]
        Task AddTag([AliasAs("id")] string buildId, [Body] string tag);

        [Put("/app/rest/builds/{id}/comment/")]
        Task SetComment([AliasAs("id")] string buildId, [Body] string comment);

        [Put("/app/rest/builds/{id}/tags/")]
        Task ReplaceTags([AliasAs("id")] string buildId, [Body] TagsDto tags);

        [Put("/app/rest/builds/{id}/pin/")]
        Task Pin([AliasAs("id")] string buildId, [Body] string comment);

        //The standard DELETE annotation doesn't allow to include a body, so we need to use our own.
        //Probably it would be better to change Rest API here (https://youtrack.jetbrains.com/issue/TW-49178).
        // @DELETE_WITH_BODY("/app/rest/builds/{id}/pin/")
        [Delete("/app/rest/builds/{id}/pin/")]
        Task Unpin([AliasAs("id")] string buildId, [Body] string comment);

        [Get("/app/rest/builds/{id}/artifacts/content/{**path}")]
        Task<Stream> ArtifactContent(
            [AliasAs("id")] string buildId,
            [AliasAs("path")] string artifactPath);

        [Headers("Accept: application/json")]
        [Get("/app/rest/builds/{id}/artifacts/children/{**path}")]
        Task<ArtifactFileListDto> ArtifactChildren(
            [AliasAs("id")] string buildId,
            [AliasAs("path")] string artifactPath,
            string locator,
            string fields);

        [Headers("Accept: application/json")]
        [Get("/app/rest/builds/{id}/resulting-properties")]
        Task<ParametersDto> ResultingProperties([AliasAs("id")] string buildId);

        [Headers("Accept: application/json")]
        [Get("/app/rest/projects/{id}")]
        Task<ProjectDto> Project(string id);

        [Headers("Accept: application/json")]
        [Get("/app/rest/buildTypes/{id}")]
        Task<BuildTypeDto> BuildConfiguration([AliasAs("id")] string buildTypeId);

        [Headers("Accept: application/json")]
        [Get("/app/rest/buildTypes/{id}/buildTags")]
        Task<TagsDto> BuildTypeTags([AliasAs("id")] string buildTypeId);

        [Headers("Accept: application/json")]
        [Get("/app/rest/buildTypes/{id}/triggers")]
        Task<TriggersDto> BuildTypeTriggers([AliasAs("id")] string buildTypeId);

        [Headers("Accept: application/json")]
        [Get("/app/rest/buildTypes/{id}/artifact-dependencies")]
        Task<ArtifactDependenciesDto> BuildTypeArtifactDependencies([AliasAs("id")] string buildTypeId);

        [Put("/app/rest/projects/{id}/parameters/{name}")]
        Task SetProjectParameter([AliasAs("id")] string projectId, string name, [Body] string value);

        [Put("/app/rest/buildTypes/{id}/parameters/{name}")]
        Task SetBuildTypeParameter([AliasAs("id")] string buildTypeId, string name, [Body] string value);

        [Put("/app/rest/buildTypes/{id}/settings/{name}")]
        Task SetBuildTypeSettings([AliasAs("id")] string buildTypeId, string name, [Body] string value);

        [Headers("Accept: application/json")]
        [Post("/app/rest/buildQueue")]
        Task<TriggeredBuildDto> TriggerBuild([Body] TriggerBuildRequestDto value);

        [Headers("Accept: application/json")]
        [Post("/app/rest/builds/{id}")]
        Task CancelBuild([AliasAs("id")] string buildId, [Body] BuildCancelRequestDto value);

        [Headers("Accept: application/json")]
        [Post("/app/rest/buildQueue/{id}")]
        Task RemoveQueuedBuild([AliasAs("id")] string buildId, [Body] BuildCancelRequestDto value);

        [Headers("Accept: application/json")]
        [Get("/app/rest/users")]
        Task<UserListDto> Users();

        [Headers("Accept: application/json")]
        [Get("/app/rest/users/{locator}")]
        Task<UserDto> Users([AliasAs("locator")] string userLocator);

        [Headers("Accept: application/json")]
        [Get("/app/rest/agents")]
        Task<BuildAgentsDto> Agents();

        [Headers("Accept: application/json")]
        [Get("/app/rest/agents/{locator}")]
        Task<BuildAgentDto> Agent([AliasAs("locator")] string agentlocator);


        [Headers("Accept: text/plain", "Content-Type: text/plain")]
        [Put("/app/rest/agents/{locator}/enabled")]
        Task EnableAgent([AliasAs("locator")] string agentlocator, [Body(BodySerializationMethod.Serialized)] bool enable);

        [Headers("Accept: application/json")]
        [Get("/app/rest/agentPools")]
        Task<BuildAgentPoolsDto> AgentPools();

        [Headers("Accept: application/json")]
        [Get("/app/rest/agentPools/{locator}")]
        Task<BuildAgentPoolDto> AgentPool([AliasAs("locator")] string agentPoolLocator);

        [Headers("Accept: application/json")]
        [Get("/app/rest/problemOccurrences")]
        Task<BuildProblemOccurrencesDto> ProblemOccurrences(string locator, string fields);

        [Headers("Accept: application/json", "Content-Type: application/xml")]
        [Post("/app/rest/projects")]
        Task<ProjectDto> CreateProject([Body] string projectDescriptionXml);

        [Headers("Accept: application/json")]
        [Delete("/app/rest/projects/{locator}")]
        Task DeleteProject([AliasAs("locator")] string projectLocator);

        [Headers("Accept: application/json", "Content-Type: application/xml")]
        [Post("/app/rest/vcs-roots")]
        Task<VcsRootDto> CreateVcsRoot([Body] string vcsRootXml);

        [Headers("Accept: application/json")]
        [Delete("/app/rest/vcs-roots/{locator}")]
        Task DeleteVcsRoot([AliasAs("locator")] string vcsRootLocator);

        [Headers("Accept: application/json", "Content-Type: application/xml")]
        [Post("/app/rest/buildTypes")]
        Task<BuildTypeDto> CreateBuildType([Body] string buildTypeXml);

        [Get("/downloadBuildLog.html")]
        Task<Stream> BuildLog([AliasAs("buildId")] string id);

        [Headers("Accept: application/json")]
        [Get("/app/rest/changes/{id},{version}")]
        Task<ChangeDto> Change([AliasAs("id")] string buildType, [AliasAs("version")] string version);

        [Headers("Accept: application/json")]
        [Get("/app/rest/changes/{id}")]
        Task<ChangeDto> Change([AliasAs("id")] string changeId);

        [Headers("Accept: application/json")]
        [Get("/app/rest/changes/{id}/firstBuilds")]
        Task<BuildListDto> ChangeFirstBuilds([AliasAs("id")] string id);

        [Headers("Accept: application/json")]
        [Get("/app/rest/changes")]
        Task<ChangesDto> Changes(string locator, string fields);
    }
}