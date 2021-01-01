using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Api
{
    /// <summary>
    /// Represents the TeamCity CI/CD system by JetBrains.
    /// </summary>
    public abstract class TeamCity
    {
        public abstract Task<IBuild> Build(BuildId id);
        public abstract Task<IBuild> Build(BuildConfigurationId buildConfigurationId, string number);
        public abstract IBuildAgentLocator BuildAgents { get; }
        public abstract IBuildAgentPoolLocator BuildAgentPools { get; }
        public abstract Task<IBuildConfiguration> BuildConfiguration(string id);
        public abstract Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id);
        public abstract IBuildQueue BuildQueue { get; }
        public abstract IBuildLocator Builds { get; }
        public abstract IChangeLocator Changes { get; }
        public abstract IInvestigationLocator Investigations { get; }
        public abstract Task<IProject> Project(ProjectId id);
        public abstract IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId projectId);
        public abstract Task<IProject> RootProject();
        public abstract ITestRunsLocator TestRuns { get; }
        public abstract IUserLocator Users { get; }
        public abstract IVcsRootLocator VcsRoots { get; }
    }
}