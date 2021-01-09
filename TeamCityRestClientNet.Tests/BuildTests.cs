using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Xunit;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tests;
using TeamCityRestClientNet.RestApi;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TeamCityRestClientNet.Builds
{
    public class Builds : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_builds_end_point()
        {
            await _teamCity.Builds.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds");
        }
    }

    public class BuildsLocator : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_builds_end_point_with_id()
        {
            await _teamCity.Builds.ById("101");

            AssertApiCall(HttpMethod.Get, "/app/rest/builds/101");
        }

        [Fact]
        public async Task Includes_defaultFilter_by_default()
        {
            await _teamCity.Builds.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("defaultFilter")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("defaultFilter")));
        }

        [Fact]
        public async Task Includes_status_SUCCESS_locator_by_default()
        {
            await _teamCity.Builds.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("status")),
                apiCall => Assert.Equal(BuildStatus.SUCCESS.ToString(), apiCall.GetLocator("status")));
        }

        [Fact]
        public async Task Includes_any_branch_locator_with_WithAllBranches()
        {
            await _teamCity.Builds.WithAllBranches().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("branch")),
                apiCall => Assert.Equal("default:any", apiCall.GetLocator("branch")));
        }

        [Fact]
        public async Task Includes_branch_locator_with_WithBranch()
        {
            await _teamCity.Builds.WithBranch("refs/heads/development").All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("branch")),
                apiCall => Assert.Equal("refs/heads/development", apiCall.GetLocator("branch")));
        }

        [Fact]
        public async Task Includes_build_number_locator_with_WithNumber()
        {
            await _teamCity.Builds.WithNumber("12").All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("number")),
                apiCall => Assert.Equal("12", apiCall.GetLocator("number")));
        }

        [Fact]
        public async Task Includes_status_locator_with_WithStatus()
        {
            await _teamCity.Builds.WithStatus(BuildStatus.FAILURE).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("status")),
                apiCall => Assert.Equal("FAILURE", apiCall.GetLocator("status")));
        }

        [Fact]
        public async Task Includes_tag_locator_with_WithTag()
        {
            await _teamCity.Builds.WithTag("Tag").All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("tags")),
                apiCall => Assert.Equal("(Tag)", apiCall.GetLocator("tags")));
        }

        [Fact]
        public async Task Includes_until_date_locator_with_Until()
        {
            var untilDate = Utilities.ParseTeamCity("20201201T203857+0000").Value;
            await _teamCity.Builds.Until(untilDate).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("untilDate")),
                apiCall => Assert.Equal("20201201T203857+0000", apiCall.GetLocator("untilDate")));
        }

        [Fact]
        public async Task Includes_since_date_locator_with_Since()
        {
            var sinceDate = Utilities.ParseTeamCity("20201201T203857+0000").Value;
            await _teamCity.Builds.Since(sinceDate).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("sinceDate")),
                apiCall => Assert.Equal("20201201T203857+0000", apiCall.GetLocator("sinceDate")));
        }

        [Fact]
        public async Task Includes_pinned_locator_with_PinnedOnly()
        {
            await _teamCity.Builds.PinnedOnly().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("pinned")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("pinned")));
        }

        [Fact]
        public async Task Includes_canceled_locator_with_OnlyCanceled()
        {
            await _teamCity.Builds.OnlyCanceled().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("canceled")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("canceled")));
        }

        [Fact]
        public async Task Includes_canceled_any_locator_with_IncludeCanceled()
        {
            await _teamCity.Builds.IncludeCanceled().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("canceled")),
                apiCall => Assert.Equal("any", apiCall.GetLocator("canceled")));
        }

        [Fact]
        public async Task Includes_personal_locator_with_OnlyPersonal()
        {
            await _teamCity.Builds.OnlyPersonal().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("personal")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("personal")));
        }

        [Fact]
        public async Task Includes_personal_any_locator_with_IncludePersonal()
        {
            await _teamCity.Builds.IncludePersonal().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("personal")),
                apiCall => Assert.Equal("any", apiCall.GetLocator("personal")));
        }

        [Fact]
        public async Task Removed_status_locator_with_IncludeFailed()
        {
            await _teamCity.Builds.IncludeFailed().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.False(apiCall.HasLocator("status")));
        }

        [Fact]
        public async Task Includes_running_any_locator_with_IncludeRunning()
        {
            await _teamCity.Builds.IncludeRunning().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("running")),
                apiCall => Assert.Equal("any", apiCall.GetLocator("running")));
        }

        [Fact]
        public async Task Includes_running_locator_with_OnlyRunning()
        {
            await _teamCity.Builds.OnlyRunning().All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("running")),
                apiCall => Assert.Equal("true", apiCall.GetLocator("running")));
        }

        [Fact]
        public async Task Includes_build_type_locator_with_FromBuildType()
        {
            await _teamCity.Builds.FromBuildType(new Id("104")).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("buildType")),
                apiCall => Assert.Equal("104", apiCall.GetLocator("buildType")));
        }

        [Fact]
        public async Task Includes_max_page_size_as_count_locator_when_PageSize_and_LimitResult_are_over_1024()
        {
            await _teamCity.Builds.PageSize(2000).LimitResults(2000).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("1024", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_limit_result_as_count_locator_when_LimitResult_is_given_without_PageSize_and_below_1024()
        {
            await _teamCity.Builds.LimitResults(1000).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("1000", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_page_size_as_count_locator_when_PageSize_is_given_without_LimitResult_and_below_1024()
        {
            await _teamCity.Builds.PageSize(1000).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("1000", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_limit_result_as_count_locator_when_both_LimitResult_and_PageSize_are_given_and_below_1024()
        {
            await _teamCity.Builds.LimitResults(500).PageSize(600).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("500", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_count_1_locator_with_Latest()
        {
            await _teamCity.Builds.Latest();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("count")),
                apiCall => Assert.Equal("1", apiCall.GetLocator("count")));
        }

        [Fact]
        public async Task Includes_snapshot_dependency_locator_with_SnapshotDependencyTo()
        {
            await _teamCity.Builds.SnapshotDependencyTo(new Id("501")).All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("snapshotDependency")),
                apiCall => Assert.Equal("(to:(id:501))", apiCall.GetLocator("snapshotDependency")));
        }

        [Fact]
        public async Task Includes_vcs_revision_locator_with_WithVcsRevision()
        {
            await _teamCity.Builds.WithVcsRevision("qwerty").All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds",
                apiCall => Assert.True(apiCall.HasLocator("revision")),
                apiCall => Assert.Equal("qwerty", apiCall.GetLocator("revision")));
        }

    }

    public class ExistingBuild : TestsBase
    {
        [Fact]
        public async Task Can_be_tagged_by_POSTing_to_builds_tags_end_point_with_id_and_tag()
        {
            var build = await _teamCity.Builds.ById("101");

            await build.AddTag("NewTag");

            AssertApiCall(HttpMethod.Post, "/app/rest/builds/101/tags",
                apiCall => Assert.Equal("NewTag", apiCall.Content));
        }

        [Fact]
        public async Task Artifact_content_can_be_streamed_by_GETting_build_artifacts_content_end_point_with_id_and_artifact_path()
        {
            var build = await _teamCity.Builds.ById("101");

            using (var ms = new MemoryStream())
            {
                await build.DownloadArtifact("mypath.txt", ms);
            }
            
            AssertApiCall(HttpMethod.Get, "/app/rest/builds/101/artifacts/content/mypath.txt");
        }

        [Fact]
        public async Task Artifact_children_can_be_retrieved_by_GETting_build_artifacts_children_end_point_with_id_and_artifact_path()
        {
            var build = await _teamCity.Builds.ById("101");

            using (var ms = new MemoryStream())
            {
                await build.GetArtifacts("parent/");
            }

            AssertApiCall(HttpMethod.Get, "/app/rest/builds/101/artifacts/children/parent/",
                apiCall => Assert.True(apiCall.HasLocator("recursive")),
                apiCall => Assert.Equal("false", apiCall.GetLocator("recursive")),
                apiCall => Assert.True(apiCall.HasLocator("hidden")),
                ApiCall => Assert.Equal("false", ApiCall.GetLocator("hidden")));
        }

        [Fact]
        public async Task Properties_can_be_retrieved_by_GETting_build_resulting_properties_end_point_with_id()
        {
            var build = await _teamCity.Builds.ById("101");

            await build.GetResultingParameters();

            AssertApiCall(HttpMethod.Get, "/app/rest/builds/101/resulting-properties");
        }

        [Fact]
        public async Task Can_be_commented_on_by_PUTting_to_builds_comment_end_point_with_id_and_comment()
        {
            var build = await _teamCity.Builds.ById("101");

            await build.SetComment("Comment");

            AssertApiCall(HttpMethod.Put, "/app/rest/builds/101/comment/",
                apiCall => Assert.Equal("Comment", apiCall.Content));
        }

        [Fact]
        public async Task Can_have_its_tags_replaces_by_PUTting_to_builds_tags_end_point_with_id_and_tags()
        {
            var build = await _teamCity.Builds.ById("101");

            await build.ReplaceTags(new List<string> { "Tag1", "Tag2"});

            AssertApiCall(HttpMethod.Put, "/app/rest/builds/101/tags/",
                apiCall => {
                    var body = apiCall.JsonContentAs<TagsDto>();
                    Assert.Collection(body.Tag,
                        tag => Assert.Equal("Tag1", tag.Name),
                        tag => Assert.Equal("Tag2", tag.Name));
                });
        }

        [Fact]
        public async Task Can_be_pinned_by_PUTting_to_builds_pin_end_point_with_id()
        {
            var build = await _teamCity.Builds.ById("101");

            await build.Pin();

            AssertApiCall(HttpMethod.Put, "/app/rest/builds/101/pin/");
        }

        [Fact]
        public async Task Can_be_unpinned_by_DELETEing_to_builds_pin_end_point_with_id()
        {
            var build = await _teamCity.Builds.ById("101");

            await build.Unpin();

            AssertApiCall(HttpMethod.Delete, "/app/rest/builds/101/pin/");
        }

    }

    public class NewBuild : TestsBase
    {
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

    public class RunningBuild : TestsBase
    {
        [Fact]
        public async Task Can_be_canceled_by_POSTing_to_builds_end_point_with_id()
        {
            var build = await _teamCity.Builds.ById("105");
            var comment = $"Cancelled-{Guid.NewGuid()}";

            await build.Cancel(comment);

            AssertApiCall(HttpMethod.Post, "/app/rest/builds/105",
                apiCall => {
                    var body = apiCall.JsonContentAs<BuildCancelRequestDto>();
                    Assert.Equal(comment, body.Comment);
                });
        }
    }
}