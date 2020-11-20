using System;
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
            this._dto = dto;
        }

        public string Version => this._dto.Version.SelfOrNullRefException();
        public string VcsBranchName 
            => this._dto.VcsBranchName.SelfOrNullRefException();
        public IVcsRootInstance VcsRootInstance 
            => new VcsRootInstance(this._dto.VcsRootInstance.SelfOrNullRefException());
    }
}