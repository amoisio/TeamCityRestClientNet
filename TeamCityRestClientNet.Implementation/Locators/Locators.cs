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

    class BuildAgentPoolLocator : Locator, IBuildAgentPoolLocator
    {
        public BuildAgentPoolLocator(TeamCityInstance instance) : base(instance) { }

        public async Task<IEnumerable<IBuildAgentPool>> All()
        {
            var pools = await _instance.Service.AgentPools().ConfigureAwait(false);
            var tasks = pools.AgentPool.Select(pool => BuildAgentPool.Create(pool.Id, _instance));
            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }

    abstract class Locator 
    {
        protected readonly TeamCityInstance _instance;

        public Locator(TeamCityInstance instance)
        {
            this._instance = instance;
        }
    }

    // private class UserLocatorImpl(private val instance: TeamCityInstanceImpl): UserLocator
    // {
    //     private var id: UserId ? = null
    //     private var username: String ? = null

    //     override fun WithId(id: UserId): UserLocator
    // {
    //     this.id = id
    //         return this
    //     }
}