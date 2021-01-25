using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Domain
{
    class VcsRootInstance : Base<VcsRootInstanceDto>, IVcsRootInstance
    {
        private VcsRootInstance(VcsRootInstanceDto fullDto, TeamCityServer instance)
            : base(fullDto, instance)
        {
            
        }

        public static async Task<IVcsRootInstance> Create(VcsRootInstanceDto dto, bool isFullDto, TeamCityServer instance)
        {
            var fullDto = isFullDto
                ? dto
                : await instance.Service.VcsRootInstance(dto.Id).ConfigureAwait(false);

            return new VcsRootInstance(fullDto, instance);
        }

        public Id VcsRootId => new Id(Dto.VcsRootId.SelfOrNullRef());
        
        public override string ToString() => $"VcsRootInstanceImpl(id={VcsRootId}, name={Name})";
    }
}