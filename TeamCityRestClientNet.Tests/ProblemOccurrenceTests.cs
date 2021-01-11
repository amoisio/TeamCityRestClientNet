using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamCityRestClientNet.Tests;
using Xunit;

namespace TeamCityRestClientNet.ProblemOccurrences
{
    public class ProblemOccurrences : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_problemOccurrences_end_point()
        {
            var build = await _teamCity.Builds.ById("102");
            await build.BuildProblems().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/problemOccurrences",
                apiCall => Assert.True(apiCall.HasLocator("build")),
                apiCall => Assert.Equal("(id:102)", apiCall.GetLocator("build")));
        }
    }
}