using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Domain
{
    class VcsRootInstance : IVcsRootInstance
    {
        private readonly VcsRootInstanceDto _dto;

        public VcsRootInstance(VcsRootInstanceDto dto)
        {
            this._dto = dto;
        }

        public VcsRootId VcsRootId 
            => new VcsRootId(this._dto.VcsRootId.SelfOrNullRef());

        public string Name => this._dto.Name.SelfOrNullRef();

        public override string ToString()
            => $"VcsRootInstanceImpl(id={VcsRootId}, name={Name})";
    }
}