using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using TeamCityRestClientNet.Tools;

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

            Assert.Contains(builds, (build) => 
                build.Id.stringId == "12" 
                && build.Status == BuildStatus.SUCCESS
                && (build.Branch.Name == null || build.Branch.Name == "refs/heads/master"));

            Assert.Contains(builds, (build) => 
                build.Id.stringId == "13" 
                && build.Status == BuildStatus.FAILURE
                && (build.Branch.Name == null || build.Branch.Name == "refs/heads/master"));
        }

        [Fact]
        public async Task Can_contain_builds_from_all_branches()
        {
            var builds = await _teamCity.Builds.WithAllBranches().All().ToListAsync();

            Assert.Contains(builds, build => build.Branch.Name == null);
            Assert.Contains(builds, build => build.Branch.Name == "refs/heads/master");
            Assert.Contains(builds, build => build.Branch.Name == "refs/heads/development");
        }

        [Fact]
        public async Task Can_contain_builds_from_specific_branch_only()
        {
            var builds = await _teamCity.Builds.WithBranch("refs/heads/development").All().ToListAsync();

            Assert.All(builds, build => Assert.Equal("refs/heads/development", build.Branch.Name));
        }
            
        [Fact]
        public async Task Can_contain_builds_with_specific_build_number_only()
        {
            var builds = await _teamCity.Builds.WithNumber("12").All().ToListAsync();

            Assert.All(builds, build => Assert.Equal("12", build.BuildNumber));
        }

        [Fact]
        public async Task Can_contain_builds_with_FAILURE_build_status_only()
        {
            var builds = await _teamCity.Builds.WithStatus(BuildStatus.FAILURE).All().ToListAsync();

            Assert.All(builds, build => Assert.Equal(BuildStatus.FAILURE, build.Status));
        }

        [Fact]
        public async Task Can_contain_builds_with_SUCCESS_build_status_only()
        {
            var builds = await _teamCity.Builds.WithStatus(BuildStatus.SUCCESS).All().ToListAsync();

            Assert.All(builds, build => Assert.Equal(BuildStatus.SUCCESS, build.Status));
        }

        [Fact]
        public async Task Contains_builds_with_specific_tag()
        {
            var builds = await _teamCity.Builds.WithTag("Tag").All().ToListAsync();

            Assert.All(builds, build => Assert.Equal("25", build.BuildNumber));
        }

        // TODO: Add test case for vcs revisions
        
        [Fact]
        public async Task Contains_builds_until_a_specific_datetime()
        {
            var untilDate = Utilities.ParseTeamCity("20201201T203857+0000").Value;
            var builds = await _teamCity.Builds.Until(untilDate).All().ToListAsync();

            Assert.All(builds, build => Assert.True(untilDate > build.FinishDateTime.Value));
        }

        [Fact]
        public async Task Contains_builds_since_a_specific_datetime()
        {
            var sinceDate = Utilities.ParseTeamCity("20201201T203857+0000").Value;
            var builds = await _teamCity.Builds.Since(sinceDate).All().ToListAsync();

            Assert.All(builds, build => Assert.True(sinceDate < build.FinishDateTime.Value));
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