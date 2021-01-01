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

    public interface IVcsRootLocator
    {
        Task<IVcsRoot> VcsRoot(VcsRootId id);
        IAsyncEnumerable<IVcsRoot> All();
    }
}