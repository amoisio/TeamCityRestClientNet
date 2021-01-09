using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using Xunit;

namespace TeamCityRestClientNet.Tests.Temp
{
    public class BuildAgents : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Contains_all_build_agents()
        {
            var agents = await _teamCity.BuildAgents.All().ToListAsync();
            Assert.Collection(agents,
                (agent) => Assert.Equal("ip_172.17.0.3", agent.Name),
                (agent) => Assert.Equal("Disabled build agent", agent.Name));
        }
    }

    public class EnabledBuildAgent : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_disabled()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            await agent.Disable();

            var agents = await _teamCity.BuildAgents.All().ToListAsync();
            Assert.Contains(agents, (agent) => agent.Id.StringId == "1" && !agent.Enabled);
        }
    }

    public class DisabledBuildAgent : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_enabled()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("2"));

            await agent.Enable();

            var agents = await _teamCity.BuildAgents.All().ToListAsync();
            Assert.Contains(agents, (agent) => agent.Id.StringId == "2" && agent.Enabled);
        }
    }

    public class ExistingBuildAgent : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved()
        {
            var agent = await _teamCity.BuildAgents.ById(new Id("1"));

            var authorizedUser = await agent.AuthorizedInfo.User;
            Assert.Equal("John Doe", authorizedUser.Name);
            Assert.Equal("john.doe@mailinator.com", authorizedUser.Email);
            Assert.Equal("jodoe", authorizedUser.Username);
            Assert.NotEqual(default(DateTimeOffset), agent.AuthorizedInfo.Timestamp);
            Assert.Equal("Authorized", agent.AuthorizedInfo.Text);

            Assert.True(agent.Connected);
            Assert.True(agent.Enabled);

            var enabledUser = await agent.EnabledInfo.User;
            Assert.Equal("John Doe", enabledUser.Name);
            Assert.Equal("john.doe@mailinator.com", enabledUser.Email);
            Assert.Equal("jodoe", enabledUser.Username);
            Assert.NotEqual(default(DateTimeOffset), agent.EnabledInfo.Timestamp);
            Assert.Equal("Enabled", agent.EnabledInfo.Text);

            Assert.Equal("172.17.0.3", agent.IpAddress);
            Assert.Equal("ip_172.17.0.3", agent.Name);
            Assert.False(agent.Outdated);
            Assert.NotEmpty(agent.Parameters);
        }

        // [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgents.ById(new Id("not.found")));
        }
    }

    public class BuildAgentPools : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Contains_all_build_agent_pools()
        {
            var agentPools = await _teamCity.BuildAgentPools.All().ToListAsync();
            Assert.Collection(agentPools,
                (pool) => Assert.Equal("Default", pool.Name));
        }

    }

    public class ExistingBuildAgentPool : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved()
        {
            var agentPool = await _teamCity.BuildAgentPools.ById(new Id("0"));

            var agents = await agentPool.Agents;
            Assert.NotEmpty(agents);
            Assert.Contains(agents, a => a.Name == "ip_172.17.0.3");

            var projects = await agentPool.Projects;
            Assert.NotEmpty(projects);
            Assert.Contains(projects, a => a.Id.StringId == "TeamCityCliNet");
            Assert.Contains(projects, a => a.Id.StringId == "TeamCityRestClientNet");
        }
      
        // [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgentPools.ById(new Id("not.found")));
        }
    }

    public class BuildList : TestsBase
    {
        // [Fact]
        public async Task By_default_contains_all_builds_from_default_branch()
        {
            var defaultBranch = "refs/heads/master";
            var builds = await _teamCity.Builds.All().ToListAsync();

            Assert.Contains(builds, (build) =>
                build.Id.StringId == "12"
                && build.Status == BuildStatus.SUCCESS
                && (build.Branch.Name == null || build.Branch.Name == defaultBranch));

            Assert.Contains(builds, (build) =>
                build.Id.StringId == "13"
                && build.Status == BuildStatus.FAILURE
                && (build.Branch.Name == null || build.Branch.Name == defaultBranch));
        }

        // [Fact]
        public async Task Can_contain_builds_from_all_branches()
        {
            var builds = await _teamCity.Builds.WithAllBranches().All().ToListAsync();

            Assert.Contains(builds, build => build.Branch.Name == null);
            Assert.Contains(builds, build => build.Branch.Name == "refs/heads/master");
            Assert.Contains(builds, build => build.Branch.Name == "refs/heads/development");
        }

        // [Fact]
        public async Task Can_contain_builds_from_specific_branch_only()
        {
            var builds = await _teamCity.Builds.WithBranch("refs/heads/development").All().ToListAsync();

            Assert.All(builds, build => Assert.Equal("refs/heads/development", build.Branch.Name));
        }

        // [Fact]
        public async Task Can_contain_builds_with_specific_build_number_only()
        {
            var builds = await _teamCity.Builds.WithNumber("12").All().ToListAsync();

            Assert.All(builds, build => Assert.Equal("12", build.BuildNumber));
        }

        // [Fact]
        public async Task Can_contain_builds_with_FAILURE_build_status_only()
        {
            var builds = await _teamCity.Builds.WithStatus(BuildStatus.FAILURE).All().ToListAsync();

            Assert.All(builds, build => Assert.Equal(BuildStatus.FAILURE, build.Status));
        }

        // [Fact]
        public async Task Can_contain_builds_with_SUCCESS_build_status_only()
        {
            var builds = await _teamCity.Builds.WithStatus(BuildStatus.SUCCESS).All().ToListAsync();

            Assert.All(builds, build => Assert.Equal(BuildStatus.SUCCESS, build.Status));
        }

        // [Fact]
        public async Task Contains_builds_with_specific_tag()
        {
            var builds = await _teamCity.Builds.WithTag("Tag").All().ToListAsync();

            Assert.All(builds, build => Assert.Equal("25", build.BuildNumber));
        }

        // TODO: Add test case for vcs revisions

        // [Fact]
        public async Task Contains_builds_until_a_specific_datetime()
        {
            var untilDate = Utilities.ParseTeamCity("20201201T203857+0000").Value;
            var builds = await _teamCity.Builds.Until(untilDate).All().ToListAsync();

            Assert.All(builds, build => Assert.True(untilDate > build.FinishDateTime.Value));
        }

        // [Fact]
        public async Task Contains_builds_since_a_specific_datetime()
        {
            var sinceDate = Utilities.ParseTeamCity("20201201T203857+0000").Value;
            var builds = await _teamCity.Builds.Since(sinceDate).All().ToListAsync();

            Assert.All(builds, build => Assert.True(sinceDate < build.FinishDateTime.Value));
        }

        // [Fact]
        public async Task Contains_pinned_builds_only()
        {
            var pinned = await _teamCity.Builds.PinnedOnly().All().ToListAsync();

            Assert.All(pinned, build => Assert.NotNull(build.PinInfo));
        }

        // [Fact]
        public async Task Contains_canceled_builds_only()
        {
            var cancelled = await _teamCity.Builds.OnlyCanceled().All().ToListAsync();

            Assert.All(cancelled, build => Assert.NotNull(build.CanceledInfo));
        }

        // TODO: only personal builds
        // // [Fact]
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

        // [Fact]
        public async Task Can_include_canceled_builds()
        {
            var canceled = await _teamCity.Builds
                .WithAllBranches()
                .IncludeCanceled()
                .All().ToListAsync();

            Assert.Contains(canceled, build => build.CanceledInfo != null && build.CanceledInfo.Text == "cancel");
        }

        // [Fact]
        public async Task Can_include_failed_builds()
        {
            var canceled = await _teamCity.Builds.IncludeFailed().All().ToListAsync();

            Assert.Contains(canceled, build => build.Status == BuildStatus.FAILURE);
        }

        // TODO: include personal builds
        // // [Fact]
        // public async Task Can_include_personal_builds()
        // {
        //     var canceled = await _teamCity.Builds.IncludePersonal().All().ToListAsync();

        //     Assert.Contains(canceled, build => build.Status == BuildStatus.FAILURE);
        // }


        // // // // [Fact]
        // // // public async Task Can_contain_running_builds_only()
        // // // {
        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var newBuild = await config.RunBuild();

        // // //     var onlyRunning = await _teamCity.Builds.OnlyRunning().All().ToListAsync();

        // // //     Assert.All(onlyRunning, build => Assert.Equal(BuildState.RUNNING, build.State));
        // // // }

        // // // // [Fact]
        // // // public async Task Can_include_running_builds()
        // // // {
        // // //     await TeamCityHelpers.EnableAllAgents(_teamCity);

        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var newBuild = await config.RunBuild();

        // // //     var inclRunning = await _teamCity.Builds.IncludeRunning().All().ToListAsync();

        // // //     Assert.Contains(inclRunning, build => build.State == BuildState.RUNNING);
        // // //     Assert.Contains(inclRunning, build => build.State == BuildState.FINISHED);

        // // // }

        // // [Fact]
        // public async Task Can_contain_builds_from_specific_configuration_only()
        // {

        // }

    }

    public class NewBuild : TestsBase
    {
        private BuildState[] _buildingStates = new BuildState[]
        {
            BuildState.QUEUED, /* Queued if there are no agents to run the build */ 
            BuildState.RUNNING /* Running if agent can start the build immediatelly */
        };

        // // // // [Fact]
        // // // public async Task Can_be_started_for_a_branch()
        // // // {
        // // //     var config = await _teamCity.BuildType("TeamCityRestClientNet_RestClient");
        // // //     var build = await config.RunBuild(logicalBranchName: "refs/heads/development");

        // // //     Assert.Contains(_buildingStates, state => state == build.State);
        // // // }

        // // // // [Fact]
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
        // // // // [Fact]
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

    public class ExistingBuild : TestsBase
    {

        // [Fact]
        public async Task Can_be_retrieved_as_latest_build()
        {
            var builds = await _teamCity.Builds.All().ToListAsync();
            var latest = builds.OrderByDescending(b => Int32.Parse(b.Id.StringId)).First();

            var latestBuild = await _teamCity.Builds.Latest();

            Assert.Equal(latest.Id, latestBuild.Id);
            Assert.Equal(latest.Name, latestBuild.Name);
            Assert.Equal(latest.Status, latestBuild.Status);
            Assert.Equal(latest.State, latest.State);
        }
    }

    public class ChangeList : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Contains_all_changes()
        {
            var changes = await _teamCity.Changes.All().ToListAsync();

            Assert.Contains(changes, change => change.Id.StringId == "1" && change.Username == "jodoe");
            Assert.Contains(changes, change => change.Id.StringId == "2" && change.Username == "jodoe");
        }
    }

    public class ExistingChange : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var change = await _teamCity.Changes.ById(new Id("1"));
            Assert.Equal("Initial commit", change.Comment.Trim());
            Assert.Equal("1", change.Id.StringId);
            var user = await change.User;
            Assert.Equal("jodoe", user.Username);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("1", user.Id.StringId);
            Assert.Equal("jodoe", change.Username);
            var rootInstance = await change.VcsRootInstance;
            Assert.Equal("Bitbucket", rootInstance.Name);
            Assert.Equal("a9f57192-48d1-4e7a-b3f5-ebead0c6f8d6", change.Version);
        }

        // [Fact]
        public async Task Throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Changes.ById(new Id("999991")));
        }
    }

    public class RootProject : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved()
        {
            var rootProject = await _teamCity.Projects.RootProject();

            Assert.Equal("_Root", rootProject.Id.StringId);
            Assert.Equal("<Root project>", rootProject.Name);
        }

        [Obsolete]
        // [Fact]
        public async Task Child_projects_can_be_retrieved()
        {
            var rootProject = await _teamCity.Projects.RootProject();

            var childProjects = await rootProject.ChildProjects;

            Assert.Contains(childProjects, p => p.Name == "TeamCity CLI .NET");
            Assert.Contains(childProjects, p => p.Id.StringId == "TeamCityCliNet");
            Assert.Contains(childProjects, p => p.Name == "TeamCity Rest Client .NET");
            Assert.Contains(childProjects, p => p.Id.StringId == "TeamCityRestClientNet");
        }
    }

    public class NewProject : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_created_as_child_to_root_project()
        {
            var project = await _teamCity.Projects.RootProject();

            var projectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            var returnedProject = await project.CreateProject(new Id(projectId), projectId);

            var newProject = await _teamCity.Projects.ById(new Id(projectId));
            Assert.NotNull(newProject);
            Assert.Equal(projectId, newProject.Name);
            Assert.Equal(projectId, returnedProject.Name);
        }

        // [Fact]
        public async Task Creation_throws_ApiException_if_id_is_invalid()
        {
            var project = await _teamCity.Projects.RootProject();

            var projectId = $"---{Guid.NewGuid().ToString().Replace('-', '_')}";
            await Assert.ThrowsAsync<Refit.ApiException>(() => project.CreateProject(new Id(projectId), projectId));
        }

        // [Fact]
        public async Task Creation_throws_ApiException_if_id_already_exists()
        {
            var project = await _teamCity.Projects.RootProject();

            var projectId = "TeamCityRestClientNet";
            await Assert.ThrowsAsync<Refit.ApiException>(() => project.CreateProject(new Id(projectId), projectId));
        }

        [Obsolete]
        // [Fact]
        public async Task Can_be_created_as_child_to_nonroot_project()
        {
            var root = await _teamCity.Projects.RootProject();
            var project = (await root.ChildProjects).First();

            var newProjectId = $"Project_{Guid.NewGuid().ToString().Replace('-', '_')}";
            await project.CreateProject(new Id(newProjectId), newProjectId);

            project = await _teamCity.Projects.ById(project.Id);
            var childProjects = await project.ChildProjects;
            Assert.Contains(childProjects, p => p.Name == newProjectId);
        }
    }

    public class ExistingProject : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            Assert.Equal("TeamCity Rest Client .NET", project.Name);
        }

        // [Fact]
        public async Task Retrieval_throws_ApiException_if_id_is_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Projects.ById(new Id("Not.Found")));
        }

        [Obsolete]
        // [Fact]
        public async Task Parameters_can_be_retrieved()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            Assert.Collection(project.Parameters,
                (param) => Assert.Equal("configuration_parameter", param.Name));
        }

        [Obsolete]
        // [Fact]
        public async Task Existing_parameter_value_can_be_changed()
        {
            var project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));

            var newValue = Guid.NewGuid().ToString();
            await project.SetParameter("configuration_parameter", newValue);

            project = await _teamCity.Projects.ById(new Id("TeamCityRestClientNet"));
            Assert.Collection(project.Parameters,
                (param) =>
                {
                    Assert.Equal("configuration_parameter", param.Name);
                    Assert.Equal(newValue, param.Value);
                });
        }

        [Obsolete]
        // [Fact]
        public async Task Can_be_deleted()
        {
            var rootProject = await _teamCity.Projects.RootProject();
            var childProjects = await rootProject.ChildProjects;

            var toDelete = childProjects
                .First(project => project.Id.StringId.StartsWith("Project_"));

            await toDelete.Delete();

            rootProject = await _teamCity.Projects.RootProject();
            childProjects = await rootProject.ChildProjects;

            Assert.DoesNotContain(childProjects, project => project.Id.StringId == toDelete.Id.StringId);
        }
    }

    public class UserList : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Contains_all_users()
        {
            var users = await _teamCity.Users.All().ToListAsync();

            Assert.Collection(users,
                user => Assert.Equal("jodoe", user.Username),
                user => Assert.Equal("jadoe", user.Username),
                user => Assert.Equal("dunkin", user.Username),
                user => Assert.Equal("maccheese", user.Username));
        }
    }

    public class ExistingUser : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var userId = new Id("1");
            var user = await _teamCity.Users.ById(userId);
            Assert.Equal(userId, user.Id);
            Assert.Equal("john.doe@mailinator.com", user.Email);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("jodoe", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=1", user.GetHomeUrl());
        }

        // [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            var userId = new Id("9999");
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Users.ById(userId));
        }

        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved_with_exact_username()
        {
            var user = await _teamCity.Users.ByUsername("dunkin");
            Assert.Equal(new Id("3"), user.Id);
            Assert.Equal("dunkin@mailinator.com", user.Email);
            Assert.Equal("Dunkin' Donuts", user.Name);
            Assert.Equal("dunkin", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=3", user.GetHomeUrl());
        }

        // [Fact]
        public async Task Throws_ApiException_if_exact_username_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Users.ByUsername("not.found"));
        }
    }

    public class VcsRootList : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Contains_all_VcsRoots()
        {
            var vcsRoots = await _teamCity.VcsRoots.All().ToListAsync();
            Assert.Contains(vcsRoots, (root) => root.Id.StringId == "TeamCityRestClientNet_Bitbucket");
        }
    }

    public class NewGitVcsRoot : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_created_for_root_project()
        {
            var project = await _teamCity.Projects.RootProject();

            var vcsId = $"Vcs_{Guid.NewGuid().ToString().Replace('-', '_')}";

            await project.CreateVcsRoot(
                new Id(vcsId),
                vcsId,
                VcsRootType.GIT,
                new Dictionary<string, string>());

            var vcs = await _teamCity.VcsRoots.ById(vcsId);
            Assert.Equal(vcsId, vcs.Id.StringId);
            Assert.Equal(vcsId, vcs.Name);
            // TODO: Add vcs root type check
            // TODO: Add parameter checks
        }

        // [Fact]
        public async Task Creation_throws_ApiException_if_id_is_invalid()
        {
            var project = await _teamCity.Projects.RootProject();

            var vcsRootId = "-----TeamCityRestClientNet_Bitbucket";
            await Assert.ThrowsAsync<Refit.ApiException>(
                () => project.CreateVcsRoot(new Id(vcsRootId), vcsRootId, VcsRootType.GIT, new Dictionary<string, string>()));
        }

        // [Fact]
        public async Task Creation_throws_ApiException_if_id_already_exists()
        {
            var project = await _teamCity.Projects.RootProject();

            var vcsRootId = "TeamCityRestClientNet_Bitbucket";
            await Assert.ThrowsAsync<Refit.ApiException>(
                () => project.CreateVcsRoot(new Id(vcsRootId), vcsRootId, VcsRootType.GIT, new Dictionary<string, string>()));
        }
    }

    public class ExistingVcsRoot : TestsBase
    {
        [Obsolete]
        // [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var rootId = new Id("TeamCityRestClientNet_Bitbucket");
            var root = await _teamCity.VcsRoots.ById(rootId);
            Assert.Equal(rootId.StringId, root.Id.StringId);
            Assert.Equal("Bitbucket", root.Name);
            Assert.Equal("refs/heads/master", root.DefaultBranch);
            Assert.Equal("https://noexist@bitbucket.org/joedoe/teamcityrestclientnet.git", root.Url);
        }

        // [Fact]
        public async Task Retrieval_throws_ApiException_if_id_not_found()
        {
            var rootId = new Id("Not_found");

            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.VcsRoots.ById(rootId));
        }

        [Obsolete]
        // [Fact]
        public async Task Can_be_deleted()
        {
            var vcsRoots = await _teamCity.VcsRoots.All().ToListAsync();

            var toDelete = vcsRoots
                .First(root => root.Id.StringId.StartsWith("Vcs_"));

            await toDelete.Delete();

            vcsRoots = await _teamCity.VcsRoots.All().ToListAsync();

            Assert.DoesNotContain(vcsRoots, root => root.Id == toDelete.Id);
        }
    }
}