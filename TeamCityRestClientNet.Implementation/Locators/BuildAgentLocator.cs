using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class BuildAgentLocator : Locator, IBuildAgentLocator
    {
        public BuildAgentLocator(TeamCityServer instance) : base(instance) { }

        /// <summary>
        /// Retrieve a build agent from TeamCity by its id.
        /// </summary>
        /// <param name="id">Id of the build agent to retrieve.</param>
        /// <returns>Matching build agent. Throws a Refit.ApiException if build agent not found.</returns>
        public async Task<IBuildAgent> BuildAgent(BuildAgentId id)
            => await Domain.BuildAgent.Create(id.stringId, Instance).ConfigureAwait(false);

        public async Task<IEnumerable<IBuildAgent>> All()
        {
            var agents = await Service.Agents().ConfigureAwait(false);
            var tasks = agents.Agent.Select(agent => Domain.BuildAgent.Create(agent.Id, Instance));
            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}