using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Locators
{
    class BuildConfigurationLocator : Locator, IBuildConfigurationLocator
    {
        public BuildConfigurationLocator(TeamCityServer instance) : base(instance)
        {

        }

        public async Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id)
            => await Domain.BuildConfiguration.Create(id.stringId, Instance).ConfigureAwait(false);

        public IAsyncEnumerable<IBuildConfiguration> All()
        {
            throw new System.NotImplementedException();
        }
    }
}