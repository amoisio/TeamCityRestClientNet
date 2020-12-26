using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Domain
{
    class VcsRootInstance : IVcsRootInstance
    {
        private readonly VcsRootInstanceDto _dto;

        public VcsRootInstance(VcsRootInstanceDto dto)
        {
            _dto = dto;
        }

        public VcsRootId VcsRootId => new VcsRootId(_dto.VcsRootId.SelfOrNullRef());
        public string Name => _dto.Name.SelfOrNullRef();
        public override string ToString()
            => $"VcsRootInstanceImpl(id={VcsRootId}, name={Name})";
    }
}