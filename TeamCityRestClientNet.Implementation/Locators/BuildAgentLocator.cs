using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class BuildAgentLocator : Locator, IBuildAgentLocator
    {
        public BuildAgentLocator(TeamCityInstance instance) : base(instance) { }

        public async Task<IEnumerable<IBuildAgent>> All()
        {
            var agents = await _instance.Service.Agents().ConfigureAwait(false);
            var tasks = agents.Agent.Select(agent => BuildAgent.Create(agent.Id, _instance));
            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}