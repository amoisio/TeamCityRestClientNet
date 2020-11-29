using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class BuildAgentPoolLocator : Locator, IBuildAgentPoolLocator
    {
        public BuildAgentPoolLocator(TeamCityServer instance) : base(instance) { }

        public async Task<IEnumerable<IBuildAgentPool>> All()
        {
            var pools = await Service.AgentPools().ConfigureAwait(false);
            var tasks = pools.AgentPool.Select(pool => BuildAgentPool.Create(pool.Id, Instance));
            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}