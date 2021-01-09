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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgents.ById(new Id("not.found")));
        }
    }

    public class BuildAgentPools : TestsBase
    {
        [Obsolete]
        [Fact]
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
        [Fact]
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
      
        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.BuildAgentPools.ById(new Id("not.found")));
        }
    }

    public class UserList : TestsBase
    {
        [Obsolete]
        [Fact]
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
        [Fact]
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

        [Fact]
        public async Task Throws_ApiException_if_id_not_found()
        {
            var userId = new Id("9999");
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Users.ById(userId));
        }

        [Obsolete]
        [Fact]
        public async Task Can_be_retrieved_with_exact_username()
        {
            var user = await _teamCity.Users.ByUsername("dunkin");
            Assert.Equal(new Id("3"), user.Id);
            Assert.Equal("dunkin@mailinator.com", user.Email);
            Assert.Equal("Dunkin' Donuts", user.Name);
            Assert.Equal("dunkin", user.Username);
            Assert.Equal($"{_serverUrl}/admin/editUser.html?userId=3", user.GetHomeUrl());
        }

        [Fact]
        public async Task Throws_ApiException_if_exact_username_not_found()
        {
            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.Users.ByUsername("not.found"));
        }
    }

    public class VcsRootList : TestsBase
    {
        [Obsolete]
        [Fact]
        public async Task Contains_all_VcsRoots()
        {
            var vcsRoots = await _teamCity.VcsRoots.All().ToListAsync();
            Assert.Contains(vcsRoots, (root) => root.Id.StringId == "TeamCityRestClientNet_Bitbucket");
        }
    }

    public class NewGitVcsRoot : TestsBase
    {
        [Obsolete]
        [Fact]
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

        [Fact]
        public async Task Creation_throws_ApiException_if_id_is_invalid()
        {
            var project = await _teamCity.Projects.RootProject();

            var vcsRootId = "-----TeamCityRestClientNet_Bitbucket";
            await Assert.ThrowsAsync<Refit.ApiException>(
                () => project.CreateVcsRoot(new Id(vcsRootId), vcsRootId, VcsRootType.GIT, new Dictionary<string, string>()));
        }

        [Fact]
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
        [Fact]
        public async Task Can_be_retrieved_with_id()
        {
            var rootId = new Id("TeamCityRestClientNet_Bitbucket");
            var root = await _teamCity.VcsRoots.ById(rootId);
            Assert.Equal(rootId.StringId, root.Id.StringId);
            Assert.Equal("Bitbucket", root.Name);
            Assert.Equal("refs/heads/master", root.DefaultBranch);
            Assert.Equal("https://noexist@bitbucket.org/joedoe/teamcityrestclientnet.git", root.Url);
        }

        [Fact]
        public async Task Retrieval_throws_ApiException_if_id_not_found()
        {
            var rootId = new Id("Not_found");

            await Assert.ThrowsAsync<Refit.ApiException>(() => _teamCity.VcsRoots.ById(rootId));
        }

        [Obsolete]
        [Fact]
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