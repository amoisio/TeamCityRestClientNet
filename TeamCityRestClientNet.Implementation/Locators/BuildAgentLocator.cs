using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class BuildAgentLocator : Locator<IBuildAgent>, IBuildAgentLocator
    {
        public BuildAgentLocator(TeamCityServer instance) : base(instance) { }

        /// <summary>
        /// Retrieve a build agent from TeamCity by its id.
        /// </summary>
        /// <param name="id">Id of the build agent to retrieve.</param>
        /// <returns>Matching build agent. Throws a Refit.ApiException if build agent not found.</returns>
        public override async Task<IBuildAgent> ById(Id id)
            => await Domain.BuildAgent.Create(id.StringId, Instance).ConfigureAwait(false);

        public override async IAsyncEnumerable<IBuildAgent> All(string initialLocator = null)
        {
            var agents = await Service.Agents().ConfigureAwait(false);
            foreach(var agent in agents.Items) 
            {
                yield return await Domain.BuildAgent.Create(agent.Id, Instance).ConfigureAwait(false);
            }
        }
    }
}