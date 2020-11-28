using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Api
{
    public struct BuildId
    {
        public BuildId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
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
}