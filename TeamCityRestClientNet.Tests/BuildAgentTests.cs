using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.BuildAgents
{
    [Collection("TeamCity Collection")]
    public class BuildAgentList : _TestsBase
    {
        public BuildAgentList(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Contains_all_build_agents()
        {
            var agents = await _teamCity.BuildAgents.All();
            Assert.Collection(agents,
                (agent) => Assert.Equal("ip_172.17.0.3", agent.Name));
        }
    }

    [Collection("TeamCity Collection")]
    public class ExistingBuildAgent : _TestsBase
    {
        public ExistingBuildAgent(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_disabled()
        {
            await TeamCityHelpers.EnableAllAgents(_teamCity).ConfigureAwait(false);

            await TeamCityHelpers.DisableAllAgents(_teamCity).ConfigureAwait(false);
            var agents = await _teamCity.BuildAgents.All();

            Assert.All(agents, agent => Assert.False(agent.Enabled));

            await TeamCityHelpers.EnableAllAgents(_teamCity).ConfigureAwait(false);
        }

        [Fact]
        public async Task Can_be_enabled()
        {
            await TeamCityHelpers.DisableAllAgents(_teamCity).ConfigureAwait(false);

            await TeamCityHelpers.EnableAllAgents(_teamCity).ConfigureAwait(false);
            var agents = await _teamCity.BuildAgents.All();

            Assert.All(agents, agent => Assert.True(agent.Enabled));
        }

        // TODO: Reimplement once end-point single agent query is supported in client        
        // [Fact]
        // public async Task BuildAgents_includes_test_system_agent()
        // {
        //     var agents = await _teamCity.BuildAgents.All();
        //     var agent = agents.First();

        //     var authorizedUser = await agent.AuthorizedInfo.User;
        //     Assert.Equal("Aleksi Moisio", authorizedUser.Name);
        //     Assert.Equal("aleksi.moisio30@gmail.com", authorizedUser.Email);
        //     Assert.Equal("amoisio", authorizedUser.Username);
        //     Assert.NotEqual(default(DateTimeOffset), agent.AuthorizedInfo.Timestamp);

        //     Assert.True(agent.Connected);
        //     Assert.True(agent.Enabled);

        //     var enabledUser = await agent.EnabledInfo.User;
        //     Assert.Equal("Aleksi Moisio", enabledUser.Name);
        //     Assert.Equal("aleksi.moisio30@gmail.com", enabledUser.Email);
        //     Assert.Equal("amoisio", enabledUser.Username);
        //     Assert.NotEqual(default(DateTimeOffset), agent.EnabledInfo.Timestamp);
        //     Assert.Equal("Enabled", agent.EnabledInfo.Text);

        //     Assert.Equal("172.17.0.3", agent.IpAddress);
        //     Assert.Equal("ip_172.17.0.3", agent.Name);
        //     Assert.False(agent.Outdated);
        //     Assert.NotEmpty(agent.Parameters);
        // }
    }
}