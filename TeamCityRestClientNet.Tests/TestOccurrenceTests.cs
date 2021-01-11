using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using Xunit;

namespace TeamCityRestClientNet.TestOccurrences
{
    public class TestOccurrences : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_testOccurrences_end_point()
        {
            await _teamCity.TestRuns.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences");
        }
    }

    public class TestRunsLocator : TestsBase
    {
        [Fact]
        public async Task Includes_status_ignored_locator_by_default()
        {
            await _teamCity.TestRuns.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("ignored")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("ignored")));
        }

        [Fact]
        public async Task Includes_expandInvocations_locator_by_default()
        {
            await _teamCity.TestRuns.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("expandInvocations")),
                apiCall => Assert.Equal("false", apiCall.GetLocator("expandInvocations")));
        }

        [Fact]
        public async Task Includes_status_locator_with_WithStatus()
        {
            await _teamCity.TestRuns.WithStatus(TestStatus.SUCCESSFUL).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("status")),
                apiCall => Assert.Equal("SUCCESS", apiCall.GetLocator("status")));
        }

        [Fact]
        public async Task Includes_build_locator_with_ForBuild()
        {
            await _teamCity.TestRuns.ForBuild(new Id("101")).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("build")),
                apiCall => Assert.Equal("101", apiCall.GetLocator("build")));
        }

        [Fact]
        public async Task Includes_affectedProject_locator_with_ForProject()
        {
            await _teamCity.TestRuns.ForProject(new Id("TeamCityProject")).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("affectedProject")),
                apiCall => Assert.Equal("TeamCityProject", apiCall.GetLocator("affectedProject")));
        }

        [Fact]
        public async Task Includes_test_locator_with_ForTest()
        {
            await _teamCity.TestRuns.ForTest(new Id("123")).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("test")),
                apiCall => Assert.Equal("123", apiCall.GetLocator("test")));
        }

        [Fact]
        public async Task Includes_count_locator_with_LimitResults()
        {
            await _teamCity.TestRuns.LimitResults(103).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("103", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_expandInvocations_locator_with_ExpandMultipleInvocations()
        {
            await _teamCity.TestRuns.ExpandMultipleInvocations().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/testOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("expandInvocations")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("expandInvocations")));
        }
    }
}