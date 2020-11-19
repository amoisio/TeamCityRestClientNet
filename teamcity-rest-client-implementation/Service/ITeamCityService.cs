using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Service
{
    internal interface ITeamCityService 
    {
        string ServerUrlBase { get; }

        // @Streaming
        // @Headers("Accept: application/json")
        // @GET("/{path}")
        Task<T> Root<T>(/*@Path("path", encode = false)*/string path);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/builds")
        Task<BuildListDto> Builds(/*@Query("locator")*/string buildLocator);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/buildQueue")
        BuildListDto QueuedBuilds(/*@Query("locator")*/string locator);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/builds/id:{id}")
        Task<BuildDto> Build(/*@Path("id")*/string id);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/investigations")
        InvestigationListDto Investigations(/*@Query("locator")*/string investigationLocator);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/investigations/id:{id}")
        InvestigationDto Investigation(/*@Path("id")*/string id);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/changes")
        Task<ChangesDto> Changes(/*@Query("locator")*/string locator,/*@Query("fields")*/string fields);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/testOccurrences/")
        TestOccurrencesDto TestOccurrences(/*@Query("locator")*/string locator,/*@Query("fields")*/string fields);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/vcs-roots")
        VcsRootListDto VcsRoots(/*@Query("locator")*/string locator = null);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/vcs-roots/id:{id}")
        Task<VcsRootDto> VcsRoot(/*@Path("id")*/string id);

        // @POST("/app/rest/builds/id:{id}/tags/")
        Task AddTag(/*@Path("id")*/string buildId,/*@Body*/string tag);

        // // @PUT("/app/rest/builds/id:{id}/comment/")
        // HttpResponse SetComment(/*@Path("id")*/string buildId,/*@Body*/string comment);

        // // @PUT("/app/rest/builds/id:{id}/tags/")
        // HttpResponse ReplaceTags(/*@Path("id")*/string buildId,/*@Body*/ TagsBean tags);

        // // @PUT("/app/rest/builds/id:{id}/pin/")
        // HttpResponse Pin(/*@Path("id")*/string buildId,/*@Body*/ string comment);

        // //The standard DELETE annotation doesn't allow to include a body, so we need to use our own.
        // //Probably it would be better to change Rest API here (https://youtrack.jetbrains.com/issue/TW-49178).
        // // @DELETE_WITH_BODY("/app/rest/builds/id:{id}/pin/")
        // HttpResponse Unpin(/*@Path("id")*/string buildId,/*@Body*/string comment);

        // // @Streaming
        // // @GET("/app/rest/builds/id:{id}/artifacts/content/{path}")
        // HttpResponse ArtifactContent(/*@Path("id")*/string buildId,/*@Path("path", encode = false)*/string artifactPath);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/builds/id:{id}/artifacts/children/{path}")
        ArtifactFileListDto ArtifactChildren(
            /*@Path("id")*/string buildId,
            /*@Path("path", encode = false)*/string artifactPath,
            /*@Query("locator")*/string locator,
            /*@Query("fields")*/string fields);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/builds/id:{id}/resulting-properties")
        ParametersDto ResultingProperties(/*@Path("id")*/string buildId);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/projects/id:{id}")
        Task<ProjectDto> Project(/*@Path("id")*/string id);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/buildTypes/id:{id}")
        Task<BuildTypeDto> BuildConfiguration(/*@Path("id")*/string buildTypeId);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/buildTypes/id:{id}/buildTags")
        Task<TagsDto> BuildTypeTags(/*@Path("id")*/string buildTypeId);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/buildTypes/id:{id}/triggers")
        Task<TriggersDto> BuildTypeTriggers(/*@Path("id")*/string buildTypeId);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/buildTypes/id:{id}/artifact-dependencies")
        Task<ArtifactDependenciesDto> BuildTypeArtifactDependencies(/*@Path("id")*/string buildTypeId);

        // @PUT("/app/rest/projects/id:{id}/parameters/{name}")
        Task SetProjectParameter(/*@Path("id")*/string projectId,/*@Path("name")*/string name,/*@Body*/string value);

        // // @PUT("/app/rest/buildTypes/id:{id}/parameters/{name}")
        Task SetBuildTypeParameter(/*@Path("id")*/string buildTypeId,/*@Path("name")*/string name,/*@Body*/string value);

        // // @PUT("/app/rest/buildTypes/id:{id}/settings/{name}")
        Task SetBuildTypeSettings(/*@Path("id")*/string buildTypeId,/*@Path("name")*/string name,/*@Body*/string value);

        // @Headers("Accept: application/json")
        // @POST("/app/rest/buildQueue")
        Task<TriggeredBuildDto> TriggerBuild(/*@Body*/TriggerBuildRequestDto value);

        // @Headers("Accept: application/json")
        // @POST("/app/rest/builds/id:{id}")
        Task CancelBuild(/*@Path("id")*/string buildId,/*@Body*/BuildCancelRequestDto value);

        // // @Headers("Accept: application/json")
        // // @POST("/app/rest/buildQueue/id:{id}")
        // HttpResponse RemoveQueuedBuild(/*@Path("id")*/string buildId,/*@Body*/BuildCancelRequestBean value);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/users")
        UserListDto Users();

        // @Headers("Accept: application/json")
        // @GET("/app/rest/users/{userLocator}")
        Task<UserDto> Users(/*@Path("userLocator")*/string userLocator);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/agents")
        BuildAgentsDto Agents();

        // @Headers("Accept: application/json")
        // @GET("/app/rest/agentPools")
        BuildAgentPoolsDto AgentPools();

        // @Headers("Accept: application/json")
        // @GET("/app/rest/agents/{locator}")
        Task<BuildAgentDto> Agents(/*@Path("locator")*/string agentLocator = null);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/agentPools/{locator}")
        Task<BuildAgentPoolDto> AgentPools(/*@Path("locator")*/string agentLocator = null);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/problemOccurrences")
        BuildProblemOccurrencesDto ProblemOccurrences(/*@Query("locator")*/string locator,/*@Query("fields")*/string fields);

        // @POST("/app/rest/projects")
        // @Headers("Accept: application/json", "Content-Type: application/xml")
        Task<ProjectDto> CreateProject(/*@Body*/string projectDescriptionXml);

        // @POST("/app/rest/vcs-roots")
        // @Headers("Accept: application/json", "Content-Type: application/xml")
        Task<VcsRootDto> CreateVcsRoot(/*@Body*/string vcsRootXml);

        // @POST("/app/rest/buildTypes")
        // @Headers("Accept: application/json", "Content-Type: application/xml")
        Task<BuildTypeDto> CreateBuildType(/*@Body*/string buildTypeXml);

        // @Streaming
        // @GET("/downloadBuildLog.html")
        // HttpResponse BuildLog(/*@Query ("buildId")*/string id);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/changes/buildType:{id},version:{version}")
        ChangeDto Change(/*@Path("id")*/string buildType,/*@Path("version")*/string version);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/changes/id:{id}")
        Task<ChangeDto> Change(/*@Path("id")*/string changeId);

        // @Headers("Accept: application/json")
        // @GET("/app/rest/changes/{id}/firstBuilds")
        BuildListDto ChangeFirstBuilds(/*@Path("id")*/string id);
    }
}

