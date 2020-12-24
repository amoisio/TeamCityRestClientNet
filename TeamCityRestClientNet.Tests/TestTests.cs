using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.Users
{
    public class TestsTests : IClassFixture<TestFixture>
    {
        protected readonly TeamCity _teamCity;
        protected readonly string _serverUrl;
        public TestsTests(TestFixture teamCityFixture)
        {
            _teamCity = teamCityFixture.TeamCity;
            _serverUrl = teamCityFixture.serverUrl;
        }

        [Fact]
        public async Task T()
        {
            var users = await _teamCity.Users().ToListAsync();
            Assert.False(true);
        }

        [Fact]
        public async Task A()
        {
            await _teamCity.User(new UserId("1"));

            // await _teamCity.User(new UserId("1"))
            //    .Expect


            // var _tc = new SafeWrapper(_teamCity);

            // _tc.User(new UserId("1"))
        }
    }
}