using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public struct VcsRootId
    {
        public VcsRootId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface IVcsRoot
    {
        VcsRootId Id { get; }
        string Name { get; }
        string Url { get; }
        string DefaultBranch { get; }
        Task Delete();
    }
    public interface IVcsRootLocator
    {
        IAsyncEnumerable<IVcsRoot> All();
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

    public interface IVcsRootInstance
    {
        VcsRootId VcsRootId { get; }
        string Name { get; }
    }

    public interface IRevision
    {
        string Version { get; }
        string VcsBranchName { get; }
        IVcsRootInstance VcsRootInstance { get; }
    }

    public interface IBranch
    {
        string Name { get; }
        bool IsDefault { get; }
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
}