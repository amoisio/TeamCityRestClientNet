using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.BuildQueue
{
    public class QueuedBuilds: TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_buildQueue_end_point()
        {
            await _teamCity.BuildQueue.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/buildQueue");
        }

        [Fact]
        public async Task Can_be_retrieved_for_a_project_by_GETting_buildQueue_endpoint_with_projectId()
        {
            await _teamCity.BuildQueue.All(new Id("TeamCityRestClientNet")).ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/buildQueue", 
                apiCall => Assert.Contains("locator", apiCall.QueryParameters.Keys.ToArray()),
                apiCall => Assert.Equal("project:TeamCityRestClientNet", apiCall.QueryParameters["locator"][0]));
        }

        [Fact]
        public async Task Can_removed_from_queue_by_POSTing_to_buildQueue_endpoint_with_buildId()
        {
            await _teamCity.BuildQueue.RemoveBuild(new Id("103"));

            AssertApiCall(HttpMethod.Post, "/app/rest/buildQueue/103");
        }

        [Fact]
        public async Task Can_be_triggered_by_POSTing_to_buildQueue_endpoint_with_trigger_request()
        {
            var buildType = await _teamCity.BuildTypes.ById("TeamCityRestClientNet_RestClient");

            await buildType.RunBuild();

            AssertApiCall(HttpMethod.Post, "/app/rest/buildQueue",
                apiCall => {
                    var request = apiCall.JsonContentAs<TriggerBuildRequestDto>();
                    Assert.Equal("TeamCityRestClientNet_RestClient", request.BuildType.Id);
                });
        }
    }
}
