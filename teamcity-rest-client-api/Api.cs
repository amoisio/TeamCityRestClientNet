using System;
using System.Collections.Generic;
using System.IO;
using BAMCIS.Util.Concurrent;

namespace TeamCityRestClientNet
{
    public abstract class TeamCityInstance : IDisposable
    {
        private bool disposedValue;
        protected abstract string ServerUrl { get; }

        public abstract TeamCityInstance withLogResponses();
        public abstract TeamCityInstance withTimeout(long timeout, TimeUnit unit);
        public abstract BuildLocator builds();
        public abstract InvestigationLocator investigations();
        public abstract Build build(BuildId id);
        public abstract Build build(BuildConfigurationId buildConfigurationId, string number);
        public abstract BuildConfiguration buildConfiguration(BuildConfigurationId id);
        public abstract VcsRootLocator vcsRoots();
        public abstract VcsRoot vcsRoot(VcsRootId id);
        public abstract Project project(ProjectId id);
        public abstract Project rootProject();
        public abstract BuildQueue buildQueue();
        public abstract User user(UserId id);
        public abstract User user(string userName);
        public abstract UserLocator users();
        public abstract BuildAgentLocator buildAgents();
        public abstract BuildAgentPoolLocator buildAgentPools();
        public abstract TestRunsLocator testRuns();
        public abstract Change change(BuildConfigurationId buildConfigurationId, string vcsRevision);
        public abstract Change change(ChangeId id);


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TeamCityInstance()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(false disposing);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


    public struct VcsRootType
    {
        public VcsRootType(string stringType)
        {
            this.stringType = stringType;
        }

        public readonly string stringType;
        // companion object {
        //     val GIT = VcsRootType("jetbrains.git")
        // }
    }

    public interface VcsRootLocator
    {
        IEnumerable<VcsRoot> all();
    }

    public interface BuildAgentLocator
    {
        IEnumerable<BuildAgent> all();
    }

    public interface BuildAgentPoolLocator
    {
        IEnumerable<BuildAgentPool> all();
    }

    public interface UserLocator
    {
        IEnumerable<User> all();
    }

    public interface BuildLocator
    {
        BuildLocator fromConfiguration(BuildConfigurationId buildConfigurationId);

        BuildLocator withNumber(string buildNumber);

        /**
         * Filters builds to include only ones which are built on top of the specified revision.
         */
        BuildLocator withVcsRevision(string vcsRevision);

        BuildLocator snapshotDependencyTo(BuildId buildId);

        /**
         * By default only successful builds are returned, call this method to include failed builds as well.
         */
        BuildLocator includeFailed();

        /**
         * By default only finished builds are returned
         */
        BuildLocator includeRunning();
        BuildLocator onlyRunning();

        /**
         * By default canceled builds are not returned
         */
        BuildLocator includeCanceled();
        BuildLocator onlyCanceled();

        BuildLocator withStatus(BuildStatus status);
        BuildLocator withTag(string tag);

        BuildLocator withBranch(string branch);

        /**
         * By default only builds from the default branch are returned, call this method to include builds from all branches.
         */
        BuildLocator withAllBranches();

        BuildLocator pinnedOnly();

        BuildLocator includePersonal();
        BuildLocator onlyPersonal();

        BuildLocator limitResults(int count);
        BuildLocator pageSize(int pageSize);
        BuildLocator since(DateTime date);
        BuildLocator until(DateTime date);

        Build latest();
        IEnumerable<Build> all();
    }

    public interface InvestigationLocator
    {
        InvestigationLocator limitResults(int count);
        InvestigationLocator forProject(ProjectId projectId);
        InvestigationLocator withTargetType(InvestigationTargetType targetType);
        IEnumerable<Investigation> all();
    }

    public interface TestRunsLocator
    {
        TestRunsLocator limitResults(int count);
        TestRunsLocator pageSize(int pageSize);
        TestRunsLocator forBuild(BuildId buildId);
        TestRunsLocator forTest(TestId testId);
        TestRunsLocator forProject(ProjectId projectId);
        TestRunsLocator withStatus(TestStatus testStatus);

        /**
         * If expandMultipleInvocations is enabled, individual runs of tests, which were executed several
         * times in same build, are returned as separate entries.
         * By default such runs are aggregated into a single value, duration property will be the sum of durations
         * of individual runs, and status will be SUCCESSFUL if and only if all runs are successful.
         */
        TestRunsLocator expandMultipleInvocations();
        IEnumerable<TestRun> all();
    }

    public struct Name
    {
        Name(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct ProjectId
    {
        public ProjectId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct BuildId
    {
        public BuildId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct TestId
    {
        public TestId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }


    public struct ChangeId
    {
        public ChangeId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct BuildConfigurationId
    {
        public BuildConfigurationId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }


    public struct VcsRootId
    {
        public VcsRootId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct BuildProblemId
    {
        public BuildProblemId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct BuildAgentPoolId
    {
        public BuildAgentPoolId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct BuildAgentId
    {
        public BuildAgentId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct InvestigationId
    {
        public InvestigationId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public struct BuildProblemType
    {
        public BuildProblemType(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;

        public bool isSnapshotDependencyError
            => this.stringId == "SNAPSHOT_DEPENDENCY_ERROR_BUILD_PROCEEDS_TYPE"
            || this.stringId == "SNAPSHOT_DEPENDENCY_ERROR";
        //         companion object
        // {
        //     val FAILED_TESTS = BuildProblemType("TC_FAILED_TESTS")
        //     }
    }

    public interface Project
    {
        ProjectId Id { get; }
        string name { get; }
        bool archived { get; }
        ProjectId? parentProjectId { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string getHomeUrl(string branch = null);
        string getTestHomeUrl(TestId testId);

        List<Project> childProjects { get; }
        List<BuildConfiguration> buildConfigurations { get; }
        List<Parameter> parameters { get; }

        void setParameter(string name, string value);

        /**
         * See properties example from existing VCS roots via inspection of the following url:
         * https://teamcity/app/rest/vcs-roots/id:YourVcsRootId
         */
        VcsRoot createVcsRoot(VcsRootId id, string name, VcsRootType type, IDictionary<string, string> properties);

        Project createProject(ProjectId id, string name);

        /**
         * XML in the same format as
         * https://teamcity/app/rest/buildTypes/YourBuildConfigurationId
         * returns
         */
        BuildConfiguration createBuildConfiguration(string buildConfigurationDescriptionXml);
    }


    public interface BuildConfiguration
    {
        BuildConfigurationId id { get; }
        string name { get; }
        ProjectId projectId { get; }
        bool paused { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string getHomeUrl(string branch = null);

        List<string> buildTags { get; }
        List<FinishBuildTrigger> finishBuildTriggers { get; }
        List<ArtifactDependency> artifactDependencies { get; }

        void setParameter(string name, string value);

        int buildCounter { get; }
        string buildNumberFormat { get; }

        Build runBuild(
            IDictionary<string, string> parameters = null,
            bool queueAtTop = false,
            bool? cleanSources = null,
            bool rebuildAllDependencies = false,
            string comment = null,
            string logicalBranchName = null,
            bool personal = false);

    }



    public interface BuildProblem
    {
        BuildProblemId id { get; }
        BuildProblemType type { get; }
        string identity { get; }
    }



    public interface BuildProblemOccurrence
    {
        BuildProblem buildProblem { get; }
        Build build { get; }
        string details { get; }
        string additionalData { get; }
    }

    public interface Parameter
    {
        string name { get; }
        string value { get; }
        bool own { get; }
    }

    public interface Branch
    {
        string name { get; }
        bool isDefault { get; }
    }

    public interface BuildCommentInfo
    {
        User user { get; }
        DateTimeOffset timestamp { get; }
        string text { get; }
    }

    public interface BuildAgentEnabledInfo
    {
        User user { get; }
        DateTimeOffset timestamp { get; }
        string text { get; }
    }

    public interface BuildAgentAuthorizedInfo
    {
        User user { get; }
        DateTimeOffset timestamp { get; }
        string text { get; }
    }

    public interface BuildCanceledInfo
    {
        User user { get; }
        DateTimeOffset cancelDateTime { get; }
        string text { get; }
    }

    public interface Build
    {
        BuildId id { get; }
        BuildConfigurationId buildConfigurationId { get; }
        string buildNumber { get; }
        BuildStatus status { get; }
        Branch branch { get; }
        BuildState state { get; }
        bool personal { get; }
        string name { get; }
        BuildCanceledInfo canceledInfo { get; }
        BuildCommentInfo comment { get; }
        bool? composite { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string getHomeUrl();

        string statusText { get; }
        DateTimeOffset queuedDateTime { get; }
        DateTimeOffset startDateTime { get; }
        DateTimeOffset finishDateTime { get; }

        BuildRunningInfo runningInfo { get; }

        List<Parameter> parameters { get; }

        List<string> tags { get; }

        /**
         * The same as revisions table on the build's Changes tab in TeamCity UI:
         * it lists the revisions of all of the VCS repositories associated with this build
         * that will be checked out by the build on the agent.
         */
        List<Revision> revisions { get; }

        /**
         * Changes is meant to represent changes the same way as displayed in the build's Changes in TeamCity UI.
         * In the most cases these are the commits between the current and previous build.
         */
        List<Change> changes { get; }

        /**
         * All snapshot-dependency-linked builds this build depends on
         */
        List<Build> snapshotDependencies { get; }

        PinInfo pinInfo { get; }

        TriggeredInfo triggeredInfo { get; }

        BuildAgent agent { get; }

        IEnumerable<TestRun> testRuns(TestStatus? status = null);

        IEnumerable<BuildProblemOccurrence> buildProblems { get; }

        void addTag(string tag);
        void setComment(string comment);
        void replaceTags(List<string> tags);
        void pin(string comment = "pinned via REST API");
        void unpin(string comment = "unpinned via REST API");
        List<BuildArtifact> getArtifacts(string parentPath = "", bool recursive = false, bool hidden = false);
        BuildArtifact findArtifact(string pattern, string parentPath = "");
        BuildArtifact findArtifact(string pattern, string parentPath = "", bool recursive = false);
        void downloadArtifacts(string pattern, File outputDir);

        void downloadArtifact(string artifactPath, Stream output);
        // void downloadArtifact(string artifactPath, OutputStream output);
        void downloadArtifact(string artifactPath, File output);
        Stream openArtifactInputStream(string artifactPath);
        void downloadBuildLog(File output);
        void cancel(string comment = "", bool reAddIntoQueue = false);
        List<Parameter> getResultingParameters();

    }


    public interface Investigation
    {
        InvestigationId id { get; }
        InvestigationState state { get; }
        User assignee { get; }
        User reporter { get; }
        string comment { get; }
        InvestigationResolveMethod resolveMethod { get; }
        InvestigationTargetType targetType { get; }
        List<TestId> testIds { get; }
        List<BuildProblemId> problemIds { get; }
        InvestigationScope scope { get; }
    }

    public interface BuildRunningInfo
    {
        int percentageComplete { get; }
        long elapsedSeconds { get; }
        long estimatedTotalSeconds { get; }
        bool outdated { get; }
        bool probablyHanging { get; }
    }

    public interface Change
    {
        ChangeId id { get; }
        string version { get; }
        string username { get; }
        User user { get; }
        DateTimeOffset dateTime { get; }
        string comment { get; }
        VcsRootInstance vcsRootInstance { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string getHomeUrl(BuildConfigurationId? specificBuildConfigurationId = null, bool? includePersonalBuilds = null);

        /**
         * Returns an uncertain amount of builds which contain the revision. The builds are not necessarily from the same
         * configuration as the revision. The feature is experimental, see https://youtrack.jetbrains.com/issue/TW-24633
         */
        List<Build> firstBuilds();
    }


    public struct UserId
    {
        public UserId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface User
    {
        UserId id { get; }
        string username { get; }
        string name { get; }
        string email { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string getHomeUrl();
    }

    public interface BuildArtifact
    {
        /** Artifact name without path. e.g. my.jar */
        string name { get; }
        /** Artifact name with path. e.g. directory/my.jar */
        string fullName { get; }
        long? size { get; }
        DateTimeOffset modificationDateTime { get; }

        Build build { get; }

        void download(File output);
        void download(Stream output);
        Stream openArtifactInputStream();
    }

    public interface VcsRoot
    {
        VcsRootId id { get; }
        string name { get; }

        string url { get; }
        string defaultBranch { get; }
    }

    public interface BuildAgent
    {
        BuildAgentId id { get; }
        string name { get; }
        BuildAgentPool pool { get; }
        bool connected { get; }
        bool enabled { get; }
        bool authorized { get; }
        bool outdated { get; }
        string ipAddress { get; }
        List<Parameter> parameters { get; }
        BuildAgentEnabledInfo enabledInfo { get; }
        BuildAgentAuthorizedInfo authorizedInfo { get; }

        Build currentBuild { get; }

        string getHomeUrl();
    }

    public interface BuildAgentPool
    {
        BuildAgentPoolId id { get; }
        string name { get; }

        List<Project> projects { get; }
        List<BuildAgent> agents { get; }
    }

    public interface VcsRootInstance
    {
        VcsRootId vcsRootId { get; }
        string name { get; }
    }

    public enum BuildStatus
    {
        SUCCESS,
        FAILURE,
        ERROR,
        UNKNOWN
    }

    public enum BuildState
    {
        QUEUED,
        RUNNING,
        FINISHED,
        DELETED,
        UNKNOWN
    }

    public enum InvestigationState
    {
        TAKEN,
        FIXED,
        GIVEN_UP
    }

    public enum InvestigationResolveMethod
    {
        MANUALLY,
        WHEN_FIXED
    }


    public interface PinInfo
    {
        User user { get; }
        DateTimeOffset dateTime { get; }
    }

    public interface Revision
    {
        string version { get; }
        string vcsBranchName { get; }
        VcsRootInstance vcsRootInstance { get; }
    }

    public enum TestStatus
    {
        SUCCESSFUL,
        IGNORED,
        FAILED,
        UNKNOWN
    }

    public interface TestRun
    {
        string name { get; }
        TestStatus status { get; }

        /**
         * Test run duration. It may be ZERO if a test finished too fast (<1ms)
         */
        TimeSpan duration { get; }

        string details { get; }
        bool ignored { get; }

        /**
         * Current 'muted' status of this test on TeamCity
         */
        bool currentlyMuted { get; }

        /**
         * Muted at the moment of running tests
         */
        bool muted { get; }

        /**
         * Newly failed test or not
         */
        bool newFailure { get; }

        BuildId buildId { get; }
        BuildId? fixedIn { get; }
        BuildId? firstFailedIn { get; }
        TestId testId { get; }
    }

    public interface TriggeredInfo
    {
        User user { get; }
        Build build { get; }
    }

    public interface FinishBuildTrigger
    {
        BuildConfigurationId initiatedBuildConfiguration { get; }
        bool afterSuccessfulBuildOnly { get; }
        HashSet<string> includedBranchPatterns { get; }
        HashSet<string> excludedBranchPatterns { get; }
    }

    public interface ArtifactDependency
    {
        BuildConfiguration dependsOnBuildConfiguration { get; }
        string branch { get; }
        List<ArtifactRule> artifactRules { get; }
        bool cleanDestinationDirectory { get; }
    }

    public interface ArtifactRule
    {
        bool include { get; }
        /**
         * Specific file, directory, or wildcards to match multiple files can be used. Ant-like wildcards are supported.
         */
        string sourcePath { get; }
        /**
         * Follows general rules for sourcePath: ant-like wildcards are allowed.
         */
        string archivePath { get; }
        /**
         * Destination directory where files are to be placed.
         */
        string destinationPath { get; }
    }


    public class TeamCityRestException : Exception
    {

        public TeamCityRestException(string message = null, Exception cause = null)
        : base(message, cause)
        {

        }
    }

    public class TeamCityQueryException : TeamCityRestException
    {
        public TeamCityQueryException(string message = null, Exception cause = null)
        : base(message, cause)
        {

        }
    }

    public class TeamCityConversationException : TeamCityRestException
    {
        public TeamCityConversationException(string message = null, Exception cause = null)
        : base(message, cause)
        {

        }
    }

    public interface BuildQueue
    {
        void removeBuild(BuildId id, string comment = "", bool reAddIntoQueue = false);
        IEnumerable<Build> queuedBuilds(ProjectId? projectId = null);
    }

    public enum InvestigationTargetType
    {
        TEST,
        BUILD_PROBLEM,
        BUILD_CONFIGURATION
    }


    public interface InvestigationScope
    {

    }

    public class InProject : InvestigationScope
    {
        private readonly Project project;
        public InProject(Project project)
        {
            this.project = project;
        }


    }
    public class InBuildConfiguration : InvestigationScope
    {
        private readonly BuildConfiguration configuration;
        public InBuildConfiguration(BuildConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}