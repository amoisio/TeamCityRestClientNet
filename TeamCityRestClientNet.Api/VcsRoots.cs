using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IVcsRoot : IIdentifiable
    {
        string Name { get; }
        string Url { get; }
        string DefaultBranch { get; }
        Task Delete();
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
        Id VcsRootId { get; }
        string Name { get; }
    }

    public interface IVcsRootLocator
    {
        Task<IVcsRoot> VcsRoot(Id id);
        IAsyncEnumerable<IVcsRoot> All();
    }
}