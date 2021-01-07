using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;
using TeamCityRestClientNet.FakeServer;

namespace TeamCityRestClientNet.BuildAgentPools
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

        [Fact]
        public async Task GETs_the_agent_pools_end_point()
        {
            await _teamCity.BuildAgentPools.All().ToListAsync();

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/agentPools");
            Assert.NotNull(apiCall);
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
        public async Task GETs_the_agent_pools_end_point_with_id()
        {
            var agent = await _teamCity.BuildAgentPools.ById(new Id("0"));

            var apiCall = GetApiCall(HttpMethod.Get, "/app/rest/agentPools/id:0");
            Assert.NotNull(apiCall);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal("0", ApiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgentPools.ById(new Id("not.found")));
        }
    }
}