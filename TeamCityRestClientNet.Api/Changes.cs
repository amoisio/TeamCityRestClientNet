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
        AsyncLazy<IVcsRootInstance> VcsRootInstance { get; }
    }

    public interface IChange : IIdentifiable
    {
        string Version { get; }
        string Username { get; }
        AsyncLazy<IUser> User { get; }
        DateTimeOffset DateTime { get; }
        string Comment { get; }
        AsyncLazy<IVcsRootInstance> VcsRootInstance { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl(Id? specificBuildTypeId = null, bool? includePersonalBuilds = null);
    }

    public interface IChangeLocator : ILocator<IChange>
    {
        Task<IChange> ByBuildTypeId(Id buildTypeId, string vcsRevision);
    }
}