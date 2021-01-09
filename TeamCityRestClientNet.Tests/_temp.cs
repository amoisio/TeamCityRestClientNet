using System;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using Xunit;

namespace TeamCityRestClientNet.Tests.Temp
{
    public class BuildAgentPoolList : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Contains_all_build_agent_pools()
        {
            var agentPools = await _teamCity.BuildAgentPools.All().ToListAsync();
            Assert.Collection(agentPools,
                (pool) => Assert.Equal("Default", pool.Name));
        }

    }

    public class ExistingBuildAgentPool : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_retrieved()
        {
            var agentPool = await _teamCity.BuildAgentPools.ById(new Id("0"));

            var agents = await agentPool.Agents;
            Assert.NotEmpty(agents);
            Assert.Contains(agents, a => a.Name == "ip_172.17.0.3");

            var projects = await agentPool.Projects;
            Assert.NotEmpty(projects);
            Assert.Contains(projects, a => a.Id.StringId == "TeamCityCliNet");
            Assert.Contains(projects, a => a.Id.StringId == "TeamCityRestClientNet");
        }
      
        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgentPools.ById(new Id("not.found")));
        }
    }

}