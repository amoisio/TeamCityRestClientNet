using System;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using Xunit;

namespace TeamCityRestClientNet.Tests.Temp
{
    public class BuildAgents : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Contains_all_build_agents()
        {
            var agents = await _teamCity.BuildAgents.All().ToListAsync();
            Assert.Collection(agents,
                (agent) => Assert.Equal("ip_172.17.0.3", agent.Name),
                (agent) => Assert.Equal("Disabled build agent", agent.Name));
        }
    }

    public class EnabledBuildAgent : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_disabled()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            await agent.Disable();

            var agents = await _teamCity.BuildAgents.All().ToListAsync();
            Assert.Contains(agents, (agent) => agent.Id.StringId == "1" && !agent.Enabled);
        }
    }

    public class DisabledBuildAgent : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_enabled()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("2"));

            await agent.Enable();

            var agents = await _teamCity.BuildAgents.All().ToListAsync();
            Assert.Contains(agents, (agent) => agent.Id.StringId == "2" && agent.Enabled);
        }
    }

    public class ExistingBuildAgent : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Can_be_retrieved()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            var authorizedUser = await agent.AuthorizedInfo.User;
            Assert.Equal("John Doe", authorizedUser.Name);
            Assert.Equal("john.doe@mailinator.com", authorizedUser.Email);
            Assert.Equal("jodoe", authorizedUser.Username);
            Assert.NotEqual(default(DateTimeOffset), agent.AuthorizedInfo.Timestamp);
            Assert.Equal("Authorized", agent.AuthorizedInfo.Text);

            Assert.True(agent.Connected);
            Assert.True(agent.Enabled);

            var enabledUser = await agent.EnabledInfo.User;
            Assert.Equal("John Doe", enabledUser.Name);
            Assert.Equal("john.doe@mailinator.com", enabledUser.Email);
            Assert.Equal("jodoe", enabledUser.Username);
            Assert.NotEqual(default(DateTimeOffset), agent.EnabledInfo.Timestamp);
            Assert.Equal("Enabled", agent.EnabledInfo.Text);

            Assert.Equal("172.17.0.3", agent.IpAddress);
            Assert.Equal("ip_172.17.0.3", agent.Name);
            Assert.False(agent.Outdated);
            Assert.NotEmpty(agent.Parameters);
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgents.ById(new Id("not.found")));
        }
    }

    public class BuildAgentPools : TestsBase
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