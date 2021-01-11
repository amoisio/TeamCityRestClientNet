using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class DataBuilder
    {
        public DataBuilder()
        {
            Builds = new BuildRepository();
            BuildAgents = new BuildAgentRepository();
            BuildAgentPools = new BuildAgentPoolRepository();
            BuildQueue = new BuildQueue();
            BuildTypes = new BuildTypeRepository();
            Changes = new ChangeRepository();
            Investigations = new InvestigationRepository();
            ProblemOccurrences = new ProblemOccurrenceRepository();
            Projects = new ProjectRepository();
            TestOccurrences = new TestOccurrenceRepository();
            Users = new UserRepository();
            VcsRoots = new VcsRootRepository();
            VcsRootInstances = new VcsRootInstanceRepository();
        }

        public void Initialize() // TODO: rename to initialize
        {
            // users
            var johnDoe = Users.CreateJohnDoe();
            var janeDoe = Users.CreateJaneDoe();
            Users.Add(johnDoe);
            Users.Add(janeDoe);

            // Vcs roots
            var vcsGit = VcsRoots.CreateVcsRestClientGit();
            var vcs1 = VcsRoots.CreateVcs(Guid.NewGuid());
            VcsRoots.Add(vcsGit);
            VcsRoots.Add(vcs1);

            // Vcs root instances
            var bitbucketInstance = VcsRootInstances.CreateVcsInstance(vcsGit);
            VcsRootInstances.Add(bitbucketInstance);

            // AgentPools
            var defaultPool = BuildAgentPools.CreateDefaultPool();
            BuildAgentPools.Add(defaultPool);

            // Agents
            var enabledAgent = BuildAgents.CreateEnabledAgent(johnDoe, johnDoe, defaultPool);
            var disabledAgent = BuildAgents.CreateDisabledAgent(janeDoe, defaultPool);
            BuildAgents.Add(enabledAgent);
            BuildAgents.Add(disabledAgent);

            var rootProject = Projects.CreateRootProject();
            var clientProject = Projects.CreateRestClientProject(rootProject);
            var cliProject = Projects.CreateTeamCityCliProject(rootProject);
            var project1 = Projects.CreateProject(Guid.NewGuid(), rootProject);
            Projects.Add(rootProject);
            Projects.Add(clientProject);
            Projects.Add(cliProject);
            Projects.Add(project1);

            var restType = BuildTypes.CreateBuildTypeRestClient(clientProject);
            var cliType = BuildTypes.CreateBuildTypeTeamCityCli(cliProject);
            BuildTypes.Add(restType);
            BuildTypes.Add(cliType);

            var buildOk = Builds.CreateOKBuild(restType, johnDoe, enabledAgent, bitbucketInstance);
            var buildFailed= Builds.CreateFailedBuild(restType, johnDoe, enabledAgent, bitbucketInstance);
            var buildQueued = Builds.CreateQueuedBuild(restType, johnDoe);
            var buildCanceled = Builds.CreateCancelledBuild(restType, johnDoe);
            var buildRunning = Builds.CreateRunningBuild(restType, johnDoe);
            Builds.Add(buildOk);
            Builds.Add(buildFailed);
            Builds.Add(buildQueued);
            Builds.Add(buildCanceled);
            Builds.Add(buildRunning);

            // Build queue
            BuildQueue.AddRange(Builds.All().Items.Where(build => build.State == "queued"));

            var change1 = Changes.CreateChange("1", "Initial commit.", DateTime.UtcNow.AddDays(-7), johnDoe, bitbucketInstance);
            var change2 = Changes.CreateChange("2", "Add TeamCity fake data.", DateTime.UtcNow.AddDays(-6), johnDoe, bitbucketInstance);
            var change3 = Changes.CreateChange("3", "Add Changes unit tests.", DateTime.UtcNow.AddDays(-4), johnDoe, bitbucketInstance);
            Changes.Add(change1);
            Changes.Add(change2);
            Changes.Add(change3);

            var inv = Investigations.CreateInvetigation("1", janeDoe);
            Investigations.Add(inv);
        }

        public BuildRepository Builds { get; private set; }
        public BuildAgentRepository BuildAgents { get; private set; }
        public BuildAgentPoolRepository BuildAgentPools { get; private set; }
        public BuildQueue BuildQueue { get; private set; }
        public BuildTypeRepository BuildTypes { get; private set; }
        public ChangeRepository Changes { get; private set; }
        public InvestigationRepository Investigations { get; private set; }
        public ProblemOccurrenceRepository ProblemOccurrences { get; set; }
        public ProjectRepository Projects { get; private set; }
        public TestOccurrenceRepository TestOccurrences { get; set; }
        public UserRepository Users { get; private set; }
        public VcsRootRepository VcsRoots { get; private set; }
        public VcsRootInstanceRepository VcsRootInstances { get; private set; }
    }
}