using System.Collections.Generic;

namespace TeamCityRestClientNet.Api
{
    public struct VcsRootType
    {
        public VcsRootType(string stringType)
        {
            this.stringType = stringType;
        }

        public readonly static VcsRootType GIT = new VcsRootType("jetbrains.git");
        public readonly string stringType;
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

    public interface IVcsRootLocator
    {
        IAsyncEnumerable<IVcsRoot> All();
    }

    public interface IVcsRoot
    {
        VcsRootId Id { get; }
        string Name { get; }
        string Url { get; }
        string DefaultBranch { get; }
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
}