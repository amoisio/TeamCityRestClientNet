using System;
using System.Collections.Generic;

namespace TeamCityRestClientNet
{
    
internal class ProjectsBean {
    List<ProjectBean> project { get; set; } = new List<ProjectBean>();
}

internal class BuildAgentsBean {
    List<BuildAgentBean> agent { get; set; } = new List<BuildAgentBean>();
}

internal class BuildAgentPoolsBean {
    List<BuildAgentPoolBean> agentPool { get; set; } = new List<BuildAgentPoolBean>();
}

internal class ArtifactFileListBean {
    List<ArtifactFileBean> file { get; set; } = new List<ArtifactFileBean>();
}

internal class ArtifactFileBean {
    string name { get; set; }
    string fullName { get; set; }
    long? size { get; set; }
    string modificationTime { get; set; }

    const string FIELDS = "${ArtifactFileBean::fullName.name},${ArtifactFileBean::name.name},${ArtifactFileBean::size.name},${ArtifactFileBean::modificationTime.name}";
}

internal class IdBean {
    string id { get; set; }
}

internal class VcsRootListBean {
    string nextHref { get; set; }
    List<VcsRootBean> vcsRoot { get; set; } = new List<VcsRootBean>();
}

internal class VcsRootBean : IdBean {
    string name { get; set; }

    NameValuePropertiesBean properties { get; set; }
}

internal class VcsRootInstanceBean {
    string vcsRootId { get; set; }
    string name { get; set; }
}

internal class BuildListBean {
    string nextHref { get; set; }
    List<BuildBean> build { get; set; } = new List<BuildBean>();
}

internal class UserListBean {
    List<UserBean> user { get; set; } = new List<UserBean>();
}

internal class BuildBean : IdBean {
    string buildTypeId { get; set; }
    BuildCanceledBean canceledInfo { get; set; }
    string number { get; set; }
    BuildStatus? status { get; set; }
    string state { get; set; }
    bool? personal { get; set; }
    string branchName { get; set; }
    bool? defaultBranch { get; set; }
    bool? composite { get; set; }

    string statusText { get; set; }
    string queuedDate { get; set; }
    string startDate { get; set; }
    string finishDate { get; set; }

    TagsBean tags { get; set; }
    BuildRunningInfoBean runningInfo { get; set; }
    RevisionsBean revisions { get; set; }

    PinInfoBean pinInfo { get; set; }

    TriggeredBean triggered { get; set; }
    BuildCommentBean comment { get; set; }
    BuildAgentBean agent { get; set; }

    ParametersBean properties { get; set; } = new ParametersBean();
    BuildTypeBean buildType { get; set; } = new BuildTypeBean();

    BuildListBean snapshotDependencies { get; set; }
}

internal class BuildRunningInfoBean {
    int percentageComplete { get; set; } = 0;
    long elapsedSeconds { get; set; } = 0;
    long estimatedTotalSeconds { get; set; } = 0;
    bool outdated { get; set; } = false;
    bool probablyHanging { get; set; } = false;
}

internal class BuildTypeBean: IdBean {
    string name { get; set; }
    string projectId { get; set; }
    bool? paused { get; set; }
    BuildTypeSettingsBean settings { get; set; }
}

internal class BuildTypeSettingsBean {
    List<NameValuePropertyBean> property { get; set; } = new List<NameValuePropertyBean>();
}

internal class BuildProblemBean {
    string id { get; set; }
    string type { get; set; }
    string identity { get; set; }
}

internal class BuildProblemOccurrencesBean {
    string nextHref { get; set; }
    List<BuildProblemOccurrenceBean> problemOccurrence { get; set; } = new List<BuildProblemOccurrenceBean>();
}

internal class BuildProblemOccurrenceBean {
    string details { get; set; }
    string additionalData { get; set; }
    BuildProblemBean problem { get; set; }
    BuildBean build { get; set; }
}

internal class BuildTypesBean {
    List<BuildTypeBean> buildType { get; set; } = new List<BuildTypeBean>();
}

internal class TagBean {
    string name { get; set; }
}

internal class TagsBean {
    List<TagBean> tag { get; set; } = new List<TagBean>();
}

internal class TriggerBuildRequestBean {
    string branchName { get; set; }
    bool? personal { get; set; }
    TriggeringOptionsBean triggeringOptions { get; set; }

    ParametersBean properties { get; set; }
    BuildTypeBean buildType { get; set; }
    CommentBean comment { get; set; }

//  TODO: lastChanges
//    <lastChanges>
//      <change id="modificationId"/>
//    </lastChanges>
}

internal class TriggeringOptionsBean {
    bool? cleanSources { get; set; }
    bool? rebuildAllDependencies { get; set; }
    bool? queueAtTop { get; set; }
}

internal class CommentBean {
    string text { get; set; }
}

internal class TriggerBean {
    string id { get; set; }
    string type { get; set; }
    ParametersBean properties { get; set; } = new ParametersBean();
}

internal class TriggersBean {
    List<TriggerBean> trigger { get; set; } = new List<TriggerBean>();
}

internal class ArtifactDependencyBean : IdBean {
    string type { get; set; }
    bool? disabled { get; set; } = false;
    bool? inherited { get; set; } = false;
    ParametersBean properties { get; set; } = new ParametersBean();
    BuildTypeBean sourceBuildType { get; set; } = new BuildTypeBean();
}

internal class ArtifactDependenciesBean {
    List<ArtifactDependencyBean> artifactDependency { get; set; } = new List<ArtifactDependencyBean>();
}

internal class ProjectBean : IdBean {
    string name { get; set; }
    string parentProjectId { get; set; }
    bool? archived { get; set; }

    ProjectsBean projects { get; set; } = new ProjectsBean();
    ParametersBean parameters { get; set; } = new ParametersBean();
    BuildTypesBean buildTypes { get; set; } = new BuildTypesBean();
}

internal class BuildAgentBean : IdBean {
    string name { get; set; }
    bool? connected { get; set; }
    bool? enabled { get; set; }
    bool? authorized { get; set; }
    bool? uptodate { get; set; }
    string ip { get; set; }

    EnabledInfoBean enabledInfo { get; set; }
    AuthorizedInfoBean authorizedInfo { get; set; }

    ParametersBean properties { get; set; }
    BuildAgentPoolBean pool { get; set; }
    BuildBean build { get; set; }
}

internal class BuildAgentPoolBean : IdBean {
    string name { get; set; }

    ProjectsBean projects { get; set; } = new ProjectsBean();
    BuildAgentsBean agents { get; set; } = new BuildAgentsBean();
}

internal class ChangesBean {
    List<ChangeBean> change { get; set; } = new List<ChangeBean>();
}

internal class ChangeBean : IdBean {
    string version { get; set; }
    UserBean user { get; set; }
    string date { get; set; }
    string comment { get; set; }
    string username { get; set; }
    VcsRootInstanceBean vcsRootInstance { get; set; }
}

internal class UserBean : IdBean {
    string username { get; set; }
    string name { get; set; }
    string email { get; set; }
}

internal class ParametersBean {
    List<ParameterBean> property { get; set; } = new List<ParameterBean>();

    public ParametersBean() {

    }

    public ParametersBean(List<ParameterBean> properties) {
        property = properties;
    }
}

internal class ParameterBean {
    string name { get; set; }
    string value { get; set; }
    bool? own { get; set; }

    ParameterBean(string name, string value) {
        this.name = name;
        this.value = value;
    }
}

internal class PinInfoBean {
    UserBean user { get; set; }
    string timestamp { get; set; }
}

internal class TriggeredBean {
    UserBean user { get; set; }
    BuildBean build { get; set; }
}

internal class BuildCommentBean {
    UserBean user { get; set; }
    string timestamp { get; set; }
    string text { get; set; }
}

internal class EnabledInfoCommentBean {
    UserBean user { get; set; }
    string timestamp { get; set; }
    string text { get; set; }
}

internal class EnabledInfoBean {
    EnabledInfoCommentBean comment { get; set; }
}

internal class AuthorizedInfoCommentBean {
    UserBean user { get; set; }
    string timestamp { get; set; }
    string text { get; set; }
}

internal class AuthorizedInfoBean {
    AuthorizedInfoCommentBean comment { get; set; }
}

internal class BuildCanceledBean {
    UserBean user { get; set; }
    string timestamp { get; set; }
    string text { get; set; }
}

internal class TriggeredBuildBean {
    int? id { get; set; }
    string buildTypeId { get; set; }
}

internal class RevisionsBean {
    List<RevisionBean> revision { get; set; } = new List<RevisionBean>();
}

internal class RevisionBean {
    string version { get; set; }
    string vcsBranchName { get; set; }
    VcsRootInstanceBean vcsRootInstance { get; set; }
}

internal class NameValuePropertiesBean {
    List<NameValuePropertyBean> property { get; set; } = new List<NameValuePropertyBean>();
}

internal class NameValuePropertyBean {
    string name { get; set; }
    string value { get; set; }
}

internal class BuildCancelRequestBean {
    string comment { get; set; } = "";
    bool readdIntoQueue { get; set; } = false;
}

internal class TestOccurrencesBean {
    string nextHref { get; set; }
    List<TestOccurrenceBean> testOccurrence { get; set; } = new List<TestOccurrenceBean>();
}

internal class TestBean {
    string id { get; set; }
}

internal class TestOccurrenceBean {
    string name { get; set; }
    string status { get; set; }
    bool? ignored { get; set; }
    long? duration { get; set; }
    string ignoreDetails { get; set; }
    string details { get; set; }
    bool? currentlyMuted { get; set; }
    bool? muted { get; set; }
    bool? newFailure { get; set; }

    BuildBean build { get; set; }
    TestBean test { get; set; }
    BuildBean nextFixed { get; set; }
    BuildBean firstFailed { get; set; }

    const string FILTER = "testOccurrence(name,status,ignored,muted,currentlyMuted,newFailure,duration,ignoreDetails,details,firstFailed(id),nextFixed(id),build(id),test(id))";
}

internal class InvestigationListBean {
    List<InvestigationBean> investigation { get; set; } = new List<InvestigationBean>();
}

internal class InvestigationBean : IdBean {
    InvestigationState? state { get; set; }
    UserBean assignee { get; set; }
    AssignmentBean assignment { get; set; }
    InvestigationResolutionBean resolution { get; set; }
    InvestigationScopeBean scope { get; set; }
    InvestigationTargetBean target { get; set; }
}

class InvestigationResolutionBean {
    string type { get; set; }
}

internal class AssignmentBean {
    UserBean user { get; set; }
    string text { get; set; }
    string timestamp { get; set; }
}

internal class InvestigationTargetBean {
    TestUnderInvestigationListBean tests { get; set; }
    ProblemUnderInvestigationListBean problems { get; set; }
    bool? anyProblem { get; set; }
}

internal class TestUnderInvestigationListBean {
    int? count { get; set; }
     List<TestBean> test { get; set; } = new  List<TestBean>();

}

internal class ProblemUnderInvestigationListBean {
    int? count { get; set; }
    List<BuildProblemBean> problem { get; set; } = new List<BuildProblemBean>();
}

internal class InvestigationScopeBean {
    BuildTypesBean buildTypes { get; set; }
    ProjectBean project { get; set; }
}



// internal interface TeamCityService {

//     @Streaming
//     @Headers("Accept: application/json")
//     @GET("/{path}")
//     fun root(@Path("path", encode = false) path: string): Response;

//     @Headers("Accept: application/json")
//     @GET("/app/rest/builds")
//     fun builds(@Query("locator") buildLocator: string): BuildListBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/buildQueue")
//     fun queuedBuilds(@Query("locator") locator: string): BuildListBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/builds/id:{id}")
//     fun build(@Path("id") id: string): BuildBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/investigations")
//     fun investigations(@Query("locator") investigationLocator: string): InvestigationListBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/investigations/id:{id}")
//     fun investigation(@Path("id") id: string): InvestigationBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/changes")
//     fun changes(@Query("locator") locator: string, @Query("fields") fields: string): ChangesBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/testOccurrences/")
//     fun testOccurrences(@Query("locator") locator: string, @Query("fields") fields: string): TestOccurrencesBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/vcs-roots")
//     fun vcsRoots(@Query("locator") locator: string = null): VcsRootListBean;

//     @Headers("Accept: application/json")
//     @GET("/app/rest/vcs-roots/id:{id}")
//     fun vcsRoot(@Path("id") id: string): VcsRootBean

//     @POST("/app/rest/builds/id:{id}/tags/")
//     fun addTag(@Path("id") buildId: string, @Body tag: Typedstring): Response

//     @PUT("/app/rest/builds/id:{id}/comment/")
//     fun setComment(@Path("id") buildId: string, @Body comment: Typedstring): Response

//     @PUT("/app/rest/builds/id:{id}/tags/")
//     fun replaceTags(@Path("id") buildId: string, @Body tags: TagsBean): Response

//     @PUT("/app/rest/builds/id:{id}/pin/")
//     fun pin(@Path("id") buildId: string, @Body comment: Typedstring): Response

//     //The standard DELETE annotation doesn't allow to include a body, so we need to use our own.
//     //Probably it would be better to change Rest API here (https://youtrack.jetbrains.com/issue/TW-49178).
//     @DELETE_WITH_BODY("/app/rest/builds/id:{id}/pin/")
//     fun unpin(@Path("id") buildId: string, @Body comment: Typedstring): Response

//     @Streaming
//     @GET("/app/rest/builds/id:{id}/artifacts/content/{path}")
//     fun artifactContent(@Path("id") buildId: string, @Path("path", encode = false) artifactPath: string): Response;

//     @Headers("Accept: application/json")
//     @GET("/app/rest/builds/id:{id}/artifacts/children/{path}")
//     fun artifactChildren(@Path("id") buildId: string,
//                          @Path("path", encode = false) artifactPath: string,;
//                          @Query("locator") locator: string,
//                          @Query("fields") fields: string): ArtifactFileListBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/builds/id:{id}/resulting-properties")
//     fun resultingProperties(@Path("id") buildId: string): ParametersBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/projects/id:{id}")
//     fun project(@Path("id") id: string): ProjectBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/buildTypes/id:{id}")
//     fun buildConfiguration(@Path("id") buildTypeId: string): BuildTypeBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/buildTypes/id:{id}/buildTags")
//     fun buildTypeTags(@Path("id") buildTypeId: string): TagsBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/buildTypes/id:{id}/triggers")
//     fun buildTypeTriggers(@Path("id") buildTypeId: string): TriggersBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/buildTypes/id:{id}/artifact-dependencies")
//     fun buildTypeArtifactDependencies(@Path("id") buildTypeId: string): ArtifactDependenciesBean

//     @PUT("/app/rest/projects/id:{id}/parameters/{name}")
//     fun setProjectParameter(@Path("id") projectId: string, @Path("name") name: string, @Body value: Typedstring): Response

//     @PUT("/app/rest/buildTypes/id:{id}/parameters/{name}")
//     fun setBuildTypeParameter(@Path("id") buildTypeId: string, @Path("name") name: string, @Body value: Typedstring): Response

//     @PUT("/app/rest/buildTypes/id:{id}/settings/{name}")
//     fun setBuildTypeSettings(@Path("id") buildTypeId: string, @Path("name") name: string, @Body value: Typedstring): Response

//     @Headers("Accept: application/json")
//     @POST("/app/rest/buildQueue")
//     fun triggerBuild(@Body value: TriggerBuildRequestBean): TriggeredBuildBean

//     @Headers("Accept: application/json")
//     @POST("/app/rest/builds/id:{id}")
//     fun cancelBuild(@Path("id") buildId: string, @Body value: BuildCancelRequestBean): Response

//     @Headers("Accept: application/json")
//     @POST("/app/rest/buildQueue/id:{id}")
//     fun removeQueuedBuild(@Path("id") buildId: string, @Body value: BuildCancelRequestBean): Response

//     @Headers("Accept: application/json")
//     @GET("/app/rest/users")
//     fun users(): UserListBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/users/{userLocator}")
//     fun users(@Path("userLocator") userLocator: string): UserBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/agents")
//     fun agents(): BuildAgentsBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/agentPools")
//     fun agentPools(): BuildAgentPoolsBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/agents/{locator}")
//     fun agents(@Path("locator") agentLocator: string = null): BuildAgentBean;

//     @Headers("Accept: application/json")
//     @GET("/app/rest/agentPools/{locator}")
//     fun agentPools(@Path("locator") agentLocator: string = null): BuildAgentPoolBean;

//     @Headers("Accept: application/json")
//     @GET("/app/rest/problemOccurrences")
//     fun problemOccurrences(@Query("locator") locator: string, @Query("fields") fields: string): BuildProblemOccurrencesBean

//     @POST("/app/rest/projects")
//     @Headers("Accept: application/json", "Content-Type: application/xml")
//     fun createProject(@Body projectDescriptionXml: Typedstring): ProjectBean

//     @POST("/app/rest/vcs-roots")
//     @Headers("Accept: application/json", "Content-Type: application/xml")
//     fun createVcsRoot(@Body vcsRootXml: Typedstring): VcsRootBean

//     @POST("/app/rest/buildTypes")
//     @Headers("Accept: application/json", "Content-Type: application/xml")
//     fun createBuildType(@Body buildTypeXml: Typedstring): BuildTypeBean

//     @Streaming
//     @GET("/downloadBuildLog.html")
//     fun buildLog(@Query ("buildId") id: string): Response

//     @Headers("Accept: application/json")
//     @GET("/app/rest/changes/buildType:{id},version:{version}")
//     fun change(@Path("id") buildType: string, @Path("version") version: string): ChangeBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/changes/id:{id}")
//     fun change(@Path("id") changeId: string): ChangeBean

//     @Headers("Accept: application/json")
//     @GET("/app/rest/changes/{id}/firstBuilds")
//     fun changeFirstBuilds(@Path("id") id: string): BuildListBean
// }

}