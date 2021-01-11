using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using System.Net.Http;

namespace TeamCityRestClientNet.Users
{
    public class Users : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_users_end_point()
        {
            var users = await _teamCity.Users.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/users");
        }
    }

    public class ExistingUser : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_users_end_point_with_id_locator()
        {
            await _teamCity.Users.ById(new Id("1"));

            AssertApiCall(HttpMethod.Get, "/app/rest/users/id:1");
        }

        [Fact]
        public async Task Can_be_retrieved_by_GETting_users_end_point_with_username_locator()
        {
            await _teamCity.Users.ByUsername("jadoe");

            AssertApiCall(HttpMethod.Get, "/app/rest/users/username:jadoe");
        }
    }
}
