using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.Builds
{
    [Collection("TeamCity Collection")]
    public class BuildList : TestsBase 
    {
        public BuildList(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task By_default_contains_all_builds_from_default_branch()
        {
            var builds = await _teamCity.Builds.All().ToListAsync();

            // TODO: BuildDto.BranchName is empty for some reason... 
            // looking at the response it looks like the only branch information 
            // is in the revisions... fix this.
            Assert.Contains(builds, (build) => 
                build.Id.stringId == "12" 
                && build.Status == BuildStatus.SUCCESS);
                // && build.Branch.Name == "refs/heads/master");

            Assert.Contains(builds, (build) => 
                build.Id.stringId == "13" 
                && build.Status == BuildStatus.FAILURE);
                // && build.Branch.Name == "refs/heads/master");
        }
    }

    [Collection("TeamCity Collection")]
    public class BuildTests : TestsBase
    {
        
        public BuildTests(TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        

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