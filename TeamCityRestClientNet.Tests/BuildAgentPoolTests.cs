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
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_agentPools_end_point()
        {
            await _teamCity.BuildAgentPools.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/agentPools");
        }
    }

    public class ExistingBuildAgentPool : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_agentPools_end_point_with_id()
        {
            var agent = await _teamCity.BuildAgentPools.ById(new Id("0"));

            AssertApiCall(HttpMethod.Get, "/app/rest/agentPools/id:0");
        }
    }
}