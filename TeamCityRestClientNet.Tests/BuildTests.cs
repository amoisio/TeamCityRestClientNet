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
    public class BuildList : _TestsBase
    {
        public BuildList(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

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

        [Fact]
        public async Task Contains_pinned_builds_only()
        {
            var pinned = await _teamCity.Builds.PinnedOnly().All().ToListAsync();

            Assert.All(pinned, build => Assert.NotNull(build.PinInfo));
        }

        [Fact]
        public async Task Contains_canceled_builds_only()
        {
            var cancelled = await _teamCity.Builds.OnlyCanceled().All().ToListAsync();

            Assert.All(cancelled, build => Assert.NotNull(build.CanceledInfo));
        }

        // TODO: only personal builds
        // [Fact]
        // public async Task Contains_personal_builds_only()
        // {
        //     // var canceled = await _teamCity.Builds.().ToListAsync();

        //     // Assert.All(canceled, async build => 
        //     // {
        //     //     Assert.NotNull(build.CanceledInfo);

        //     //     Assert.Equal("cancel", build.CanceledInfo.Text);
        //     //     var cancelTimestamp = Utilities.ParseTeamCity("20201214T201259+0000").Value;
        //     //     Assert.Equal(cancelTimestamp, build.CanceledInfo.Timestamp);
        //     //     var cancelUser = await build.CanceledInfo.User;
        //     //     Assert.Equal("aleksi.moisio30@gmail.com", cancelUser.Email);
        //     //     Assert.Equal("1", cancelUser.Id.stringId);
        //     //     Assert.Equal("Aleksi Moisio", cancelUser.Name);
        //     //     Assert.Equal("amoisio", cancelUser.Username);
        //     // });
        // }

        [Fact]
        public async Task Can_include_canceled_builds()
        {
            var canceled = await _teamCity.Builds
                .WithAllBranches()
                .IncludeCanceled()
                .All().ToListAsync();

            Assert.Contains(canceled, build => build.CanceledInfo != null && build.CanceledInfo.Text == "cancel");
        }

        [Fact]
        public async Task Can_include_failed_builds()
        {
            var canceled = await _teamCity.Builds.IncludeFailed().All().ToListAsync();

            Assert.Contains(canceled, build => build.Status == BuildStatus.FAILURE);
        }

        // TODO: include personal builds
        // [Fact]
        // public async Task Can_include_personal_builds()
        // {
        //     var canceled = await _teamCity.Builds.IncludePersonal().All().ToListAsync();

        //     Assert.Contains(canceled, build => build.Status == BuildStatus.FAILURE);
        // }


        // // // [Fact]
        // // // public async Task Can_contain_running_builds_only()
        // // // {
        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var newBuild = await config.RunBuild();

        // // //     var onlyRunning = await _teamCity.Builds.OnlyRunning().All().ToListAsync();

        // // //     Assert.All(onlyRunning, build => Assert.Equal(BuildState.RUNNING, build.State));
        // // // }

        // // // [Fact]
        // // // public async Task Can_include_running_builds()
        // // // {
        // // //     await TeamCityHelpers.EnableAllAgents(_teamCity);

        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var newBuild = await config.RunBuild();

        // // //     var inclRunning = await _teamCity.Builds.IncludeRunning().All().ToListAsync();

        // // //     Assert.Contains(inclRunning, build => build.State == BuildState.RUNNING);
        // // //     Assert.Contains(inclRunning, build => build.State == BuildState.FINISHED);

        // // // }

        // [Fact]
        // public async Task Can_contain_builds_from_specific_configuration_only()
        // {

        // }

    }

    [Collection("TeamCity Collection")]
    public class NewBuild : _TestsBase
    {
        public NewBuild(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        private BuildState[] _buildingStates = new BuildState[]
        {
            BuildState.QUEUED, /* Queued if there are no agents to run the build */ 
            BuildState.RUNNING /* Running if agent can start the build immediatelly */
        };

        // // // [Fact]
        // // // public async Task Can_be_started_for_a_branch()
        // // // {
        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var build = await config.RunBuild(logicalBranchName: "refs/heads/development");

        // // //     Assert.Contains(_buildingStates, state => state == build.State);
        // // // }

        // // // [Fact]
        // // // public async Task Can_be_seen_on_the_build_queue()
        // // // {
        // // //     await TeamCityHelpers.DisableAllAgents(_teamCity).ConfigureAwait(false);

        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var newBuild = await config.RunBuild();

        // // //     var queuedBuilds = await _teamCity.BuildQueue.QueuedBuilds().ToListAsync();

        // // //     Assert.Contains(queuedBuilds, build => build.Id.stringId == newBuild.Id.stringId);

        // // //     await TeamCityHelpers.EnableAllAgents(_teamCity).ConfigureAwait(false);
        // // // }
    }

    [Collection("TeamCity Collection")]
    public class RunningBuild : _TestsBase
    {
        public RunningBuild(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        // // // [Fact]
        // // // public async Task Can_be_cancelled()
        // // // {
        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var newBuild = await config.RunBuild();

        // // //     await Task.Delay(500).ConfigureAwait(false);

        // // //     var comment = $"Cancelled-{Guid.NewGuid()}";
        // // //     await newBuild.Cancel(comment);

        // // //     await Task.Delay(500).ConfigureAwait(false);

        // // //     var cancelledBuilds = await _teamCity.Builds.OnlyCanceled().All().ToListAsync();
        // // //     Assert.Contains(cancelledBuilds, build => build.CanceledInfo.Text == comment);
        // // // }
    }

    [Collection("TeamCity Collection")]
    public class ExistingBuild : _TestsBase
    {
        public ExistingBuild(_TeamCityFixture teamCityFixture) : base(teamCityFixture) { }

        [Fact]
        public async Task Can_be_retrieved_as_latest_build()
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