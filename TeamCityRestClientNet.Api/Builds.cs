using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IBranch
    {
        string Name { get; }
        bool IsDefault { get; }
    }

    public interface IBuild : IIdentifiable
    {
        Id BuildTypeId { get; }
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

    public interface IBuildLocator : ILocator<IBuild>
    {
        IBuildLocator FromBuildType(Id buildTypeId);
        IBuildLocator WithNumber(string buildNumber);
        /**
         * Filters builds to include only ones which are built on top of the specified revision.
         */
        IBuildLocator WithVcsRevision(string vcsRevision);
        IBuildLocator SnapshotDependencyTo(Id buildId);
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
    }

    public interface IBuildCommentInfo : IInfo { }

    public interface IBuildCanceledInfo : IInfo { }

    public interface IBuildRunningInfo
    {
        int PercentageComplete { get; }
        long ElapsedSeconds { get; }
        long EstimatedTotalSeconds { get; }
        bool Outdated { get; }
        bool ProbablyHanging { get; }
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

    public interface ITriggeredInfo
    {
        AsyncLazy<IUser> User { get; }
        AsyncLazy<IBuild> Build { get; }
    }

    public interface IPinInfo
    {
        AsyncLazy<IUser> User { get; }
        DateTimeOffset DateTime { get; }
    }
}