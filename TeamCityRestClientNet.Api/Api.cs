using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BAMCIS.Util.Concurrent;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public abstract class TeamCityInstanceBase : IDisposable
    {
        private const string factoryFQN = "TeamCityRestClientNet.TeamCityInstanceFactory";
        private bool disposedValue;
        public abstract string ServerUrl { get; }
        public abstract Task<IBuild> Build(BuildId id);
        public abstract Task<IBuild> Build(BuildConfigurationId buildConfigurationId, string number);
        public abstract IBuildAgentLocator BuildAgents { get; }
        public abstract IBuildAgentPoolLocator BuildAgentPools { get; }
        public abstract Task<IBuildConfiguration> BuildConfiguration(string id);
        public abstract Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id);
        public abstract IBuildQueue BuildQueue { get; }
        public abstract IBuildLocator Builds { get; }
        public abstract Task<IChange> Change(BuildConfigurationId buildConfigurationId, string vcsRevision);
        public abstract Task<IChange> Change(ChangeId id);
        public abstract IInvestigationLocator Investigations { get; }
        public abstract Task<IProject> Project(ProjectId id);
        public abstract IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId projectId);
        public abstract Task<IProject> RootProject();
        public abstract Task<ITestRunsLocator> TestRuns();
        public abstract Task<IUser> User(UserId id);
        public abstract Task<IUser> User(string userName);
        public abstract Task<IUserLocator> Users();
        public abstract Task<IVcsRoot> VcsRoot(VcsRootId id);
        public abstract Task<IVcsRootLocator> VcsRoots();
        public abstract TeamCityInstanceBase WithLogResponses();
        public abstract TeamCityInstanceBase WithTimeout(long timeout, TimeUnit unit);

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

        public readonly static VcsRootType GIT = new VcsRootType("jetbrains.git");
        public readonly string stringType;
    }

    public interface IVcsRootLocator
    {
        IAsyncEnumerable<IVcsRoot> All();
    }

    public interface IBuildAgentLocator
    {
        Task<IEnumerable<IBuildAgent>> All();
    }

    public interface IBuildAgentPoolLocator
    {
        Task<IEnumerable<IBuildAgentPool>> All();
    }

    public interface IUserLocator
    {
        Task<IEnumerable<IUser>> All();
    }

    public interface IBuildLocator
    {
        IBuildLocator FromConfiguration(BuildConfigurationId buildConfigurationId);

        IBuildLocator WithNumber(string buildNumber);

        /**
         * Filters builds to include only ones which are built on top of the specified revision.
         */
        IBuildLocator WithVcsRevision(string vcsRevision);

        IBuildLocator SnapshotDependencyTo(BuildId buildId);

        /**
         * By default only successful builds are returned, call this method to include failed builds as well.
         */
        IBuildLocator IncludeFailed();

        /**
         * By default only finished builds are returned
         */
        IBuildLocator IncludeRunning();
        IBuildLocator OnlyRunning();

        /**
         * By default canceled builds are not returned
         */
        IBuildLocator IncludeCanceled();
        IBuildLocator OnlyCanceled();

        IBuildLocator WithStatus(BuildStatus status);
        IBuildLocator WithTag(string tag);

        IBuildLocator WithBranch(string branch);

        /**
         * By default only builds from the default branch are returned, call this method to include builds from all branches.
         */
        IBuildLocator WithAllBranches();

        IBuildLocator PinnedOnly();

        IBuildLocator IncludePersonal();
        IBuildLocator OnlyPersonal();

        IBuildLocator LimitResults(int count);
        IBuildLocator PageSize(int pageSize);
        IBuildLocator Since(DateTimeOffset date);
        IBuildLocator Until(DateTimeOffset date);

        Task<IBuild> Latest();
        IAsyncEnumerable<IBuild> All();
    }

    public interface IInvestigationLocator
    {
        IInvestigationLocator LimitResults(int count);
        IInvestigationLocator ForProject(ProjectId projectId);
        IInvestigationLocator WithTargetType(InvestigationTargetType targetType);
        Task<IEnumerable<IInvestigation>> All();
    }

    public interface ITestRunsLocator
    {
        ITestRunsLocator LimitResults(int count);
        ITestRunsLocator PageSize(int pageSize);
        ITestRunsLocator ForBuild(BuildId buildId);
        ITestRunsLocator ForTest(TestId testId);
        ITestRunsLocator ForProject(ProjectId projectId);
        ITestRunsLocator WithStatus(TestStatus testStatus);

        /**
         * If expandMultipleInvocations is enabled, individual runs of tests, which were executed several
         * times in same build, are returned as separate entries.
         * By default such runs are aggregated into a single value, duration property will be the sum of durations
         * of individual runs, and status will be SUCCESSFUL if and only if all runs are successful.
         */
        ITestRunsLocator ExpandMultipleInvocations();
        IAsyncEnumerable<ITestRun> All();
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
        
        public readonly static BuildProblemType FAILED_TESTS = new BuildProblemType("TC_FAILED_TESTS");
    }

    public interface IProject
    {
        ProjectId Id { get; }
        string Name { get; }
        bool Archived { get; }
        ProjectId? ParentProjectId { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl(string branch = null);
        string GetTestHomeUrl(TestId testId);

        AsyncLazy<List<IProject>> ChildProjects { get; }
        AsyncLazy<List<IBuildConfiguration>> BuildConfigurations { get; }
        List<IParameter> Parameters { get; }

        Task SetParameter(string name, string value);

        /**
         * See properties example from existing VCS roots via inspection of the following url:
         * https://teamcity/app/rest/vcs-roots/id:YourVcsRootId
         */
        Task<IVcsRoot> CreateVcsRoot(VcsRootId id, string name, VcsRootType type, IDictionary<string, string> properties);

        Task<IProject> CreateProject(ProjectId id, string name);

        /**
         * XML in the same format as
         * https://teamcity/app/rest/buildTypes/YourBuildConfigurationId
         * returns
         */
        Task<IBuildConfiguration> CreateBuildConfiguration(string buildConfigurationDescriptionXml);
    }


    public interface IBuildConfiguration
    {
        BuildConfigurationId Id { get; }
        string Name { get; }
        ProjectId ProjectId { get; }
        bool Paused { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl(string branch = null);
        AsyncLazy<List<string>> BuildTags { get; }
        AsyncLazy<List<IFinishBuildTrigger>> FinishBuildTriggers { get; }
        AsyncLazy<List<IArtifactDependency>> ArtifactDependencies { get; }
        int BuildCounter { get; }
        Task SetBuildCounter(int count);
        string BuildNumberFormat { get; }
        Task SetBuildNumberFormat(string format);
        Task SetParameter<T>(string name, T value);
        Task<IBuild> RunBuild(
            IDictionary<string, string> parameters = null,
            bool queueAtTop = false,
            bool? cleanSources = null,
            bool rebuildAllDependencies = false,
            string comment = null,
            string logicalBranchName = null,
            bool personal = false);
    }

    public interface IBuildProblem
    {
        BuildProblemId Id { get; }
        BuildProblemType Type { get; }
        string Identity { get; }
    }



    public interface IBuildProblemOccurrence
    {
        IBuildProblem BuildProblem { get; }
        AsyncLazy<IBuild> Build { get; }
        string Details { get; }
        string AdditionalData { get; }
    }

    public interface IParameter
    {
        string Name { get; }
        string Value { get; }
        bool Own { get; }
    }

    public interface IBranch
    {
        string Name { get; }
        bool IsDefault { get; }
    }

    public interface IInfo 
    {
        AsyncLazy<IUser> User { get; }
        DateTimeOffset Timestamp { get; }
        string Text { get; }
    }

    public interface IBuildCommentInfo : IInfo { }

    public interface IBuildAgentEnabledInfo : IInfo { }

    public interface IBuildAgentAuthorizedInfo : IInfo { }

    public interface IBuildCanceledInfo : IInfo { }

    public interface IBuild
    {
        BuildId Id { get; }
        BuildConfigurationId BuildConfigurationId { get; }
        string BuildNumber { get; }
        BuildStatus? Status { get; }
        IBranch Branch { get; }
        BuildState State { get; }
        bool Personal { get; }
        string Name { get; }
        IBuildCanceledInfo CanceledInfo { get; }
        IBuildCommentInfo Comment { get; }
        bool? Composite { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl();

        string StatusText { get; }
        DateTimeOffset QueuedDateTime { get; }
        DateTimeOffset? StartDateTime { get; }
        DateTimeOffset? FinishDateTime { get; }

        IBuildRunningInfo RunningInfo { get; }

        List<IParameter> Parameters { get; }

        List<string> Tags { get; }

        /**
         * The same as revisions table on the build's Changes tab in TeamCity UI:
         * it lists the revisions of all of the VCS repositories associated with this build
         * that will be checked out by the build on the agent.
         */
        List<IRevision> Revisions { get; }

        /**
         * Changes is meant to represent changes the same way as displayed in the build's Changes in TeamCity UI.
         * In the most cases these are the commits between the current and previous build.
         */
        AsyncLazy<List<IChange>> Changes { get; }

        /**
         * All snapshot-dependency-linked builds this build depends on
         */
        AsyncLazy<List<IBuild>> SnapshotDependencies { get; }

        IPinInfo PinInfo { get; }

        ITriggeredInfo TriggeredInfo { get; }

        AsyncLazy<IBuildAgent> Agent { get; }

        IAsyncEnumerable<ITestRun> TestRuns(TestStatus? status = null);

        IAsyncEnumerable<IBuildProblemOccurrence> BuildProblems();

        Task AddTag(string tag);
        Task Cancel(string comment = "", bool reAddIntoQueue = false);
        Task SetComment(string comment);
        Task ReplaceTags(List<string> tags);
        Task Pin(string comment = "pinned via REST API");
        Task Unpin(string comment = "unpinned via REST API");
        Task<List<IBuildArtifact>> GetArtifacts(string parentPath = "", bool recursive = false, bool hidden = false);
        Task<IBuildArtifact> FindArtifact(string pattern, string parentPath = "");
        Task<IBuildArtifact> FindArtifact(string pattern, string parentPath = "", bool recursive = false);
        Task DownloadArtifact(string artifactPath, Stream output);
        // void DownloadArtifact(string artifactPath, OutputStream output);
        Task DownloadArtifact(string artifactPath, FileInfo outputFile);
        Task DownloadArtifacts(string pattern, DirectoryInfo outputDir);
        Task DownloadBuildLog(FileInfo outputFile);
        Task<Stream> OpenArtifactStream(string artifactPath);
        Task<List<IParameter>> GetResultingParameters();
    }

    public interface IInvestigation
    {
        InvestigationId Id { get; }
        InvestigationState State { get; }
        AsyncLazy<IUser> Assignee { get; }
        AsyncLazy<IUser> Reporter { get; }
        string Comment { get; }
        InvestigationResolveMethod ResolveMethod { get; }
        InvestigationTargetType TargetType { get; }
        List<TestId> TestIds { get; }
        List<BuildProblemId> ProblemIds { get; }
        AsyncLazy<IInvestigationScope> Scope { get; }
    }

    public interface IBuildRunningInfo
    {
        int PercentageComplete { get; }
        long ElapsedSeconds { get; }
        long EstimatedTotalSeconds { get; }
        bool Outdated { get; }
        bool ProbablyHanging { get; }
    }

    public interface IChange
    {
        ChangeId Id { get; }
        string Version { get; }
        string Username { get; }
        AsyncLazy<IUser> User { get; }
        DateTimeOffset DateTime { get; }
        string Comment { get; }
        IVcsRootInstance VcsRootInstance { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl(BuildConfigurationId? specificBuildConfigurationId = null, bool? includePersonalBuilds = null);

        /**
         * Returns an uncertain amount of builds which contain the revision. The builds are not necessarily from the same
         * configuration as the revision. The feature is experimental, see https://youtrack.jetbrains.com/issue/TW-24633
         */
        Task<List<IBuild>> FirstBuilds();
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

    public interface IUser
    {
        UserId Id { get; }
        string Username { get; }
        string Name { get; }
        string Email { get; }

        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl();
    }

    public interface IBuildArtifact
    {
        /** Artifact name without path. e.g. my.jar */
        string Name { get; }
        /** Artifact name with path. e.g. directory/my.jar */
        string FullName { get; }
        long? Size { get; }
        DateTimeOffset ModificationDateTime { get; }
        IBuild Build { get; }
        Task Download(FileInfo outputFile);
        Task Download(Stream output);
        Task<Stream> OpenArtifactInputStream();
    }

    public interface IVcsRoot
    {
        VcsRootId Id { get; }
        string Name { get; }

        string Url { get; }
        string DefaultBranch { get; }
    }

    public interface IBuildAgent
    {
        BuildAgentId Id { get; }
        string Name { get; }
        AsyncLazy<IBuildAgentPool> Pool { get; }
        bool Connected { get; }
        bool Enabled { get; }
        bool Authorized { get; }
        bool Outdated { get; }
        string IpAddress { get; }
        List<IParameter> Parameters { get; }
        IInfo EnabledInfo { get; }
        IInfo AuthorizedInfo { get; }
        AsyncLazy<IBuild> CurrentBuild { get; }
        string GetHomeUrl();
    }

    public interface IBuildAgentPool
    {
        BuildAgentPoolId Id { get; }
        string Name { get; }
        AsyncLazy<List<IProject>> Projects { get; }
        AsyncLazy<List<IBuildAgent>> Agents { get; }
    }

    public interface IVcsRootInstance
    {
        VcsRootId VcsRootId { get; }
        string Name { get; }
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


    public interface IPinInfo
    {
        AsyncLazy<IUser> User { get; }
        DateTimeOffset DateTime { get; }
    }

    public interface IRevision
    {
        string Version { get; }
        string VcsBranchName { get; }
        IVcsRootInstance VcsRootInstance { get; }
    }

    public enum TestStatus
    {
        SUCCESSFUL,
        IGNORED,
        FAILED,
        UNKNOWN
    }

    public interface ITestRun
    {
        string Name { get; }
        TestStatus Status { get; }

        /**
         * Test run duration. It may be ZERO if a test finished too fast (<1ms)
         */
        TimeSpan Duration { get; }

        string Details { get; }
        bool Ignored { get; }

        /**
         * Current 'muted' status of this test on TeamCity
         */
        bool CurrentlyMuted { get; }

        /**
         * Muted at the moment of running tests
         */
        bool Muted { get; }

        /**
         * Newly failed test or not
         */
        bool NewFailure { get; }

        BuildId BuildId { get; }
        BuildId? FixedIn { get; }
        BuildId? FirstFailedIn { get; }
        TestId TestId { get; }
    }

    public interface ITriggeredInfo
    {
        AsyncLazy<IUser> User { get; }
        AsyncLazy<IBuild> Build { get; }
    }

    public interface IFinishBuildTrigger
    {
        BuildConfigurationId InitiatedBuildConfiguration { get; }
        bool AfterSuccessfulBuildOnly { get; }
        HashSet<string> IncludedBranchPatterns { get; }
        HashSet<string> ExcludedBranchPatterns { get; }
    }

    public interface IArtifactDependency
    {
        AsyncLazy<IBuildConfiguration> DependsOnBuildConfiguration { get; }
        string Branch { get; }
        List<IArtifactRule> ArtifactRules { get; }
        bool CleanDestinationDirectory { get; }
    }

    public interface IArtifactRule
    {
        bool Include { get; }
        /**
         * Specific file, directory, or wildcards to match multiple files can be used. Ant-like wildcards are supported.
         */
        string SourcePath { get; }
        /**
         * Follows general rules for sourcePath: ant-like wildcards are allowed.
         */
        string ArchivePath { get; }
        /**
         * Destination directory where files are to be placed.
         */
        string DestinationPath { get; }
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

    public interface IBuildQueue
    {
        Task RemoveBuild(BuildId id, string comment = "", bool reAddIntoQueue = false);
        IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId? projectId = null);
    }

    public enum InvestigationTargetType
    {
        TEST,
        BUILD_PROBLEM,
        BUILD_CONFIGURATION
    }


    public interface IInvestigationScope
    {

    }

    public class InProject : IInvestigationScope
    {
        private readonly IProject project;
        public InProject(IProject project)
        {
            this.project = project;
        }


    }
    public class InBuildConfiguration : IInvestigationScope
    {
        private readonly IBuildConfiguration configuration;
        public InBuildConfiguration(IBuildConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}