using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.BuildAgents
{
    public class BuildAgentList : TestsBase, IClassFixture<TeamCityFixture>
    {
        public BuildAgentList(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Contains_all_build_agents()
        {
            var agents = await _teamCity.BuildAgents.All();
            Assert.Collection(agents,
                (agent) => Assert.Equal("ip_172.17.0.3", agent.Name),
                (agent) => Assert.Equal("Disabled build agent", agent.Name));
        }

        [Fact]
        public async Task GETs_the_agents_end_point()
        {
            var agents = await _teamCity.BuildAgents.All();

            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/agents", ApiCall.RequestPath);
        }
    }

    public class EnabledBuildAgent : TestsBase, IClassFixture<TeamCityFixture>
    {
        public EnabledBuildAgent(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_disabled()
        {
            var agent = await _teamCity.BuildAgents.Agent(new BuildAgentId("1"));

            await agent.Disable();
            
            var agents = await _teamCity.BuildAgents.All();
            Assert.Contains(agents, (agent) => agent.Id.stringId == "1" && !agent.Enabled);
        }

        [Fact]
        public async Task PUTs_the_agents_end_point_with_id_and_disabled_set()
        {
            var agent = await _teamCity.BuildAgents.Agent(new BuildAgentId("1"));

            await agent.Disable();

            Assert.Equal(HttpMethod.Put, ApiCall.Method);
            Assert.StartsWith("/app/rest/agents", ApiCall.RequestPath);
            Assert.Equal("1", ApiCall.GetLocatorValue());
            Assert.Equal("enabled", ApiCall.Property);
            Assert.Equal("false", ApiCall.Content);
        }
    }

    public class DisabledBuildAgent : TestsBase, IClassFixture<TeamCityFixture>
    {
        public DisabledBuildAgent(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_enabled()
        {
            var agent = await _teamCity.BuildAgents.Agent(new BuildAgentId("2"));

            await agent.Enable();

            var agents = await _teamCity.BuildAgents.All();
            Assert.Contains(agents, (agent) => agent.Id.stringId == "2" && agent.Enabled);
        }

        [Fact]
        public async Task PUTs_the_agents_end_point_with_id_and_enabled_set()
        {
            var agent = await _teamCity.BuildAgents.Agent(new BuildAgentId("1"));

            await agent.Enable();

            Assert.Equal(HttpMethod.Put, ApiCall.Method);
            Assert.StartsWith("/app/rest/agents", ApiCall.RequestPath);
            Assert.Equal("1", ApiCall.GetLocatorValue());
            Assert.Equal("enabled", ApiCall.Property);
            Assert.Equal("true", ApiCall.Content);
        }

    }

    public class ExistingBuildAgent : TestsBase, IClassFixture<TeamCityFixture>
    {
        public ExistingBuildAgent(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved()
        {
            var agent = await _teamCity.BuildAgents.Agent(new BuildAgentId("1"));

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
        public async Task GETs_the_agents_end_point_with_id()
        {
            var agent = await _teamCity.BuildAgents.Agent(new BuildAgentId("1"));
            Assert.Equal(HttpMethod.Get, ApiCall.Method);
            Assert.StartsWith("/app/rest/agents", ApiCall.RequestPath);
            Assert.True(ApiCall.HasLocators);
            Assert.Equal("1", ApiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgents.Agent(new BuildAgentId("not.found")));
        }
    }
}