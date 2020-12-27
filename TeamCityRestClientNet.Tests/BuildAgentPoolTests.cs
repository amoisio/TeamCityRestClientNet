using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.BuildAgentPools
{
    [Collection("TeamCity Collection")]
    public class BuildAgentPoolList : _TestsBase 
    {
        public BuildAgentPoolList(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Contains_all_build_agent_pools()
        {
            var agentPools = await _teamCity.BuildAgentPools.All();
            Assert.Collection(agentPools, 
                (pool) => Assert.Equal("Default", pool.Name));
        }
    }

    [Collection("TeamCity Collection")]
    public class ExistingBuildAgentPool : _TestsBase
    {
        public ExistingBuildAgentPool(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        // TODO: Reimplement once end-point is supported in client.
        // [Fact]
        // public async Task BuildAgentPool_includes_test_system_agent_and_projects()
        // {
        //     var agentPools = await _teamCity.BuildAgentPools.All();
        //     var defaultPool = agentPools.First();

        //     var agents = await defaultPool.Agents;
        //     Assert.NotEmpty(agents);
        //     Assert.Contains(agents, a => a.Name == "ip_172.17.0.3");

        //     var projects = await defaultPool.Projects;
        //     Assert.NotEmpty(projects);
        //     Assert.Contains(projects, a => a.Id.stringId == "TeamCityCliNet");
        //     Assert.Contains(projects, a => a.Id.stringId == "TeamCityRestClientNet");
        // }
    }
}