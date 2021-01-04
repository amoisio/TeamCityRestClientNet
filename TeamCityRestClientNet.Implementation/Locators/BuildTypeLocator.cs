using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class BuildTypeLocator : Locator<IBuildType>, IBuildTypeLocator
    {
        public BuildTypeLocator(TeamCityServer instance) : base(instance) { }

        public async override Task<IBuildType> ById(Id id)
            => await Domain.BuildType.Create(id.StringId, Instance).ConfigureAwait(false);

        public override IAsyncEnumerable<IBuildType> All(string initialLocator = null)
        {
            return new Paged2<IBuildType, BuildTypeDto, BuildTypeListDto>(
                Instance,
                () => Service.BuildTypes(),
                (dto) => Domain.BuildType.Create(dto, false, Instance)
            );
        }
    }
}