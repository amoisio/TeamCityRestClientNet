using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using Xunit;

namespace TeamCityRestClientNet.Investigations
{
    public class Investigations : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_investigations_end_point()
        {
            await _teamCity.Investigations.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/investigations");
        }
    }

    public class InvestigationsLocator : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_investigations_end_point_with_id()
        {
            await _teamCity.Investigations.ById("1");

            AssertApiCall(HttpMethod.Get, "/app/rest/investigations/1");
        }

        [Fact]
        public async Task Includes_affecterProject_locator_with_ForProject()
        {
            await _teamCity.Investigations.ForProject(new Id("Project1")).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/investigations",
                apiCall => Assert.True(apiCall.HasLocator("affectedProject")),
                apiCall => Assert.Equal("Project1", apiCall.GetLocator("affectedProject")));
        }

        [Fact]
        public async Task Includes_count_locator_with_LimitResult()
        {
            await _teamCity.Investigations.LimitResults(100).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/investigations",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("100", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_type_locator_with_WithTargetType()
        {
            await _teamCity.Investigations.WithTargetType(InvestigationTargetType.BUILD_CONFIGURATION).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/investigations",
                apiCall => Assert.True(apiCall.HasLocator("type")),
                apiCall => Assert.Equal("BUILD_CONFIGURATION", apiCall.GetLocator("type")));
        }
    }
}