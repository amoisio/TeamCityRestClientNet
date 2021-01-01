using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IRevision
    {
        string Version { get; }
        string VcsBranchName { get; }
        IVcsRootInstance VcsRootInstance { get; }
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

    public interface IChangeLocator
    {
        Task<IChange> Change(BuildConfigurationId buildConfigurationId, string vcsRevision);
        Task<IChange> Change(ChangeId id);
        IAsyncEnumerable<IChange> All();
    }
}