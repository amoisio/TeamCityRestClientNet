using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.BuildAgentPools
{
    public class BuildAgentPools : TestsBase
    {
        // [Fact]
        // public async Task Contains_all_build_agent_pools()
        // {
        //     var agentPools = await _teamCity.BuildAgentPools.All().ToListAsync();
        //     Assert.Collection(agentPools, 
        //         (pool) => Assert.Equal("Default", pool.Name));
        // }

        [Fact]
        public async Task Can_be_retrieved_by_GETtin_the_agentPools_end_point()
        {
            await _teamCity.BuildAgentPools.All().ToListAsync();

            ApiCall2
                .ExpectedRequest(HttpMethod.Get, "/app/rest/agentPools")
                .Assert();
        }
    }

    public class ExistingBuildAgentPool : TestsBase
    {
        // [Fact]
        // public async Task Can_be_retrieved()
        // {
        //     var agentPool = await _teamCity.BuildAgentPools.ById(new Id("0"));

        //     var agents = await agentPool.Agents;
        //     Assert.NotEmpty(agents);
        //     Assert.Contains(agents, a => a.Name == "ip_172.17.0.3");

        //     var projects = await agentPool.Projects;
        //     Assert.NotEmpty(projects);
        //     Assert.Contains(projects, a => a.Id.StringId == "TeamCityCliNet");
        //     Assert.Contains(projects, a => a.Id.StringId == "TeamCityRestClientNet");
        // }

        [Fact]
        public async Task Can_be_retrieved_by_GETtin_the_agentPools_end_point_with_id()
        {
            var agent = await _teamCity.BuildAgentPools.ById(new Id("0"));

            ApiCall2
                .ExpectedRequest(HttpMethod.Get, "/app/rest/agentPools/id:0")
                .Assert();
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgentPools.ById(new Id("not.found")));
        }
    }
}