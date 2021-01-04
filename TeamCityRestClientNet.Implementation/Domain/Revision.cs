using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Domain
{
    class Revision : IRevision
    {
        private readonly RevisionDto _dto;
        public Revision(RevisionDto dto, TeamCityServer instance)
        {
            _dto = dto;
            this.VcsRootInstance = new AsyncLazy<IVcsRootInstance>(async ()
                => await Domain.VcsRootInstance.Create(_dto.VcsRootInstance, false, instance).ConfigureAwait(false));
        }

        public string Version => _dto.Version.SelfOrNullRef();
        public string VcsBranchName => _dto.VcsBranchName.SelfOrNullRef();
        public AsyncLazy<IVcsRootInstance> VcsRootInstance { get; }
    }
}