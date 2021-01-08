using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.BuildAgents
{
    public class BuildAgentList : TestsBase
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

        [Fact]
        public async Task GETs_the_agents_end_point()
        {
            await _teamCity.BuildAgents.All().ToListAsync();

            var apiCall = ApiCall(HttpMethod.Get, "/app/rest/agents");
            Assert.NotNull(apiCall);
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

        [Fact]
        public async Task PUTs_the_agents_end_point_with_id_and_disabled_set()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            await agent.Disable();

            var apiCall = ApiCall(HttpMethod.Put, "/app/rest/agents/id:1/enabled");
            Assert.NotNull(apiCall);
            Assert.Equal("1", apiCall.GetLocatorValue());
            Assert.Equal("enabled", apiCall.Property);
            Assert.Equal("false", apiCall.Content);
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

        [Fact]
        public async Task PUTs_the_agents_end_point_with_id_and_enabled_set()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            await agent.Enable();

            var apiCall = ApiCall(HttpMethod.Put, "/app/rest/agents/id:1/enabled");
            Assert.NotNull(apiCall);
            Assert.Equal("1", apiCall.GetLocatorValue());
            Assert.Equal("enabled", apiCall.Property);
            Assert.Equal("true", apiCall.Content);
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
        public async Task GETs_the_agents_end_point_with_id()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            var apiCall = ApiCall(HttpMethod.Get, "/app/rest/agents/id:1");
            Assert.NotNull(apiCall);
            Assert.Equal("1", apiCall.GetLocatorValue());
        }

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgents.ById(new Id("not.found")));
        }
    }
}