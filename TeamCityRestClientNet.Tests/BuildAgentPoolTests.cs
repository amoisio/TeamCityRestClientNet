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
    public class BuildAgentPoolList : TestsBase, IClassFixture<TeamCityFixture>
    {
        public BuildAgentPoolList(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Contains_all_build_agent_pools()
        {
            var agentPools = await _teamCity.BuildAgentPools.All();
            Assert.Collection(agentPools, 
                (pool) => Assert.Equal("Default", pool.Name));
        }

        [Fact]
        public async Task GETs_the_agent_pools_end_point()
        {
            var agents = await _teamCity.BuildAgentPools.All();

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/agentPools", ApiCall.RequestPath);
        }
    }

    public class ExistingBuildAgentPool : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ExistingBuildAgentPool(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved()
        {
            var agentPool = await _teamCity.BuildAgentPools.BuildAgentPool(new BuildAgentPoolId("0"));

            var agents = await agentPool.Agents;
            Assert.NotEmpty(agents);
            Assert.Contains(agents, a => a.Name == "ip_172.17.0.3");

            var projects = await agentPool.Projects;
            Assert.NotEmpty(projects);
            Assert.Contains(projects, a => a.Id.stringId == "TeamCityCliNet");
            Assert.Contains(projects, a => a.Id.stringId == "TeamCityRestClientNet");
        }

        [Fact]
        public async Task GETs_the_agent_pools_end_point_with_id()
        {
            var agent = await _teamCity.BuildAgentPools.BuildAgentPool(new BuildAgentPoolId("0"));
            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/agentPools", ApiCall.RequestPath);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal("0", ApiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgentPools.BuildAgentPool(new BuildAgentPoolId("not.found")));
        }
    }
}