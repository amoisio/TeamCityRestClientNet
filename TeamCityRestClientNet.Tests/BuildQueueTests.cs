using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;

namespace TeamCityRestClientNet.Tests
{
    public class QueuedBuilds: TestsBase, IClassFixture<TeamCityFixture>
    {
        public QueuedBuilds(TeamCityFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Can_be_retrieved()
        {
            var queuedBuilds = await _teamCity.BuildQueue.All().ToListAsync();

            Assert.All(queuedBuilds, build => Assert.Equal(BuildState.QUEUED, build.State));
        }

        [Fact]
        public async Task GETs_the_buildQueue_end_point()
        {
            var queuedBuilds = await _teamCity.BuildQueue.All().ToListAsync();

            Assert.Equal(HttpMethod.Get, FirstApiCall.Method);
            Assert.Equal("/app/rest/buildQueue", FirstApiCall.RequestPath);
        }

        [Fact]
        public async Task GET_query_contains_given_projectId_as_query_parameter()
        {
            var queuedBuilds = await _teamCity.BuildQueue.All(new Id("TeamCityRestClientNet")).ToListAsync();

            Assert.Equal(HttpMethod.Get, FirstApiCall.Method);
            Assert.Equal("/app/rest/buildQueue", FirstApiCall.RequestPath);
            Assert.True(FirstApiCall.QueryParameters.ContainsKey("locator"));
            Assert.Equal("project:TeamCityRestClientNet", FirstApiCall.QueryParameters["locator"][0]);
        }
    }
}
