using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class BuildAgentTests : TestsBase
    {
        public BuildAgentTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task BuildAgents_All_query_returns_all_agents()
        {
            var agents = await _teamCity.BuildAgents.All();
            Assert.Contains(agents, (agent) => agent.Name == "ip_172.17.0.3");
        }

        [Fact]
        public async Task BuildAgents_includes_test_system_agent()
        {
            var agents = await _teamCity.BuildAgents.All();
            var agent = agents.First();

            var authorizedUser = await agent.AuthorizedInfo.User;
            Assert.Equal("Aleksi Moisio", authorizedUser.Name);
            Assert.Equal("aleksi.moisio30@gmail.com", authorizedUser.Email);
            Assert.Equal("amoisio", authorizedUser.Username);
            Assert.NotEqual(default(DateTimeOffset), agent.AuthorizedInfo.Timestamp);

            Assert.True(agent.Connected);
            Assert.True(agent.Enabled);

            var enabledUser = await agent.EnabledInfo.User;
            Assert.Equal("Aleksi Moisio", enabledUser.Name);
            Assert.Equal("aleksi.moisio30@gmail.com", enabledUser.Email);
            Assert.Equal("amoisio", enabledUser.Username);
            Assert.NotEqual(default(DateTimeOffset), agent.EnabledInfo.Timestamp);
            Assert.Equal("Enabled", agent.EnabledInfo.Text);

            Assert.Equal("172.17.0.3", agent.IpAddress);
            Assert.Equal("ip_172.17.0.3", agent.Name);
            Assert.False(agent.Outdated);
            Assert.NotEmpty(agent.Parameters);
        }

        [Fact]
        public async Task BuildAgentPools_All_query_returns_all_pools()
        {
            var agentPool = await _teamCity.BuildAgentPools.All();
            Assert.NotEmpty(agentPool);
        }

        [Fact]
        public async Task BuildAgentPool_includes_test_system_agent_and_projects()
        {
            var agentPools = await _teamCity.BuildAgentPools.All();
            var defaultPool = agentPools.First();

            var agents = await defaultPool.Agents;
            Assert.NotEmpty(agents);
            Assert.Contains(agents, a => a.Name == "ip_172.17.0.3");

            var projects = await defaultPool.Projects;
            Assert.NotEmpty(projects);
            Assert.Contains(projects, a => a.Id.stringId == "TeamCityCliNet");
            Assert.Contains(projects, a => a.Id.stringId == "TeamCityRestClientNet");
        }
    }
}
