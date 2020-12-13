using System;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using Xunit;

namespace TeamCityRestClientNet.Tests
{
    [Collection("TeamCity Collection")]
    public class BuildTests : TestsBase
    {
        public BuildTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task All_builds()
        {
            var builds = await _teamCity.Builds.All().ToListAsync();

            Assert.Contains(builds, (build) => build.Id.stringId == "12" && build.Status == BuildStatus.SUCCESS);
        }

        [Fact]
        public async Task Latest_build()
        {
            var builds = await _teamCity.Builds.All().ToListAsync();
            var latest = builds.OrderByDescending(b => Int32.Parse(b.Id.stringId)).First();

            var latestBuild = await _teamCity.Builds.Latest();

            Assert.Equal(latest.Id, latestBuild.Id);
            Assert.Equal(latest.Name, latestBuild.Name);
            Assert.Equal(latest.Status, latestBuild.Status);
            Assert.Equal(latest.State, latest.State);
        }
    }
}