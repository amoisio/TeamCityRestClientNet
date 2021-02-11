using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.BuildAgents
{
    public class BuildAgents : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_agents_end_point()
        {
            await _teamCity.BuildAgents.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/agents");
        }
    }

    public class EnabledBuildAgent : TestsBase
    {
        [Fact]
        public async Task Can_be_enabled_by_PUTting_the_agents_end_point_with_id_and_disabled_flag()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            await agent.Disable();

            AssertApiCall(HttpMethod.Put, "/app/rest/agents/id:1/enabled",
                (apiCall) => Assert.Equal("enabled", apiCall.PropertySegment),
                (apiCall) => Assert.Equal("false", apiCall.Content));
        }
    }

    public class DisabledBuildAgent : TestsBase
    {
        [Fact]
        public async Task Can_be_disabled_by_PUTting_the_agents_end_point_with_id_and_enabled_flag()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            await agent.Enable();

            AssertApiCall(HttpMethod.Put, "/app/rest/agents/id:1/enabled",
                (apiCall) => Assert.Equal("enabled", apiCall.PropertySegment),
                (apiCall) => Assert.Equal("true", apiCall.Content));
        }
    }

    public class ExistingBuildAgent : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_agents_end_point_with_id()
        {
            await _teamCity.BuildAgents.ById(new Id("1"));

            AssertApiCall(HttpMethod.Get, "/app/rest/agents/id:1");
        }
    }
}