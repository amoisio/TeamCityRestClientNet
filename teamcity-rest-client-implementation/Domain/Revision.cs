using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Domain
{
    class Revision : IRevision
    {
        private readonly RevisionDto _dto;

        public Revision(RevisionDto dto)
        {
            _dto = dto;
        }

        public string Version => _dto.Version.SelfOrNullRef();
        public string VcsBranchName => _dto.VcsBranchName.SelfOrNullRef();
        public IVcsRootInstance VcsRootInstance 
            => new VcsRootInstance(_dto.VcsRootInstance.SelfOrNullRef());
    }
}