using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class BuildAgentPoolLocator : Locator<IBuildAgentPool>, IBuildAgentPoolLocator
    {
        public BuildAgentPoolLocator(TeamCityServer instance) : base(instance) { }

        /// <summary>
        /// Retrieve a build agent pool from TeamCity by its id.
        /// </summary>
        /// <param name="id">Id of the build agent pool to retrieve.</param>
        /// <returns>Matching build agent. Throws a Refit.ApiException if build agent not found.</returns>
        public override async Task<IBuildAgentPool> ById(Id id)
            => await Domain.BuildAgentPool.Create(id.StringId, Instance).ConfigureAwait(false);

        public override async IAsyncEnumerable<IBuildAgentPool> All(string initialLocator = null)
        {
            var pools = await Service.AgentPools().ConfigureAwait(false);
            foreach(var pool in pools.Items)
            {
                yield return await Domain.BuildAgentPool.Create(pool.Id, Instance).ConfigureAwait(false);
            }
        }
    }
}