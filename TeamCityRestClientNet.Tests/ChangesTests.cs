using System;
using TeamCityRestClientNet.Api;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Changes
{
    public class Changes : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_changes_end_point()
        {
            await _teamCity.Changes.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/changes");
        }
    }

    public class ExistingChange : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_changes_end_point_with_id()
        {
            var change = await _teamCity.Changes.ById(new Id("1"));

            AssertApiCall(HttpMethod.Get, "/app/rest/changes/1");
        }
    }
}