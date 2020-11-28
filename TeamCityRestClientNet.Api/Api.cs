using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IUserLocator
    {
        Task<IEnumerable<IUser>> All();
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

    public interface IPinInfo
    {
        AsyncLazy<IUser> User { get; }
        DateTimeOffset DateTime { get; }
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
}