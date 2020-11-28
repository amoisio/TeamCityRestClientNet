using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BAMCIS.Util.Concurrent;

namespace TeamCityRestClientNet.Api
{
    /// <summary>
    /// Represents the TeamCity CI/CD system by JetBrains.
    /// </summary>
    public abstract class TeamCity : IDisposable
    {
        private const string factoryFQN = "TeamCityRestClientNet.TeamCityInstanceFactory";
        private bool disposedValue;
        public abstract Task<IBuild> Build(BuildId id);
        public abstract Task<IBuild> Build(BuildConfigurationId buildConfigurationId, string number);
        public abstract IBuildAgentLocator BuildAgents { get; }
        public abstract IBuildAgentPoolLocator BuildAgentPools { get; }
        public abstract Task<IBuildConfiguration> BuildConfiguration(string id);
        public abstract Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id);
        public abstract IBuildQueue BuildQueue { get; }
        public abstract IBuildLocator Builds { get; }
        public abstract Task<IChange> Change(BuildConfigurationId buildConfigurationId, string vcsRevision);
        public abstract Task<IChange> Change(ChangeId id);
        public abstract IInvestigationLocator Investigations { get; }
        public abstract Task<IProject> Project(ProjectId id);
        public abstract IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId projectId);
        public abstract Task<IProject> RootProject();
        public abstract Task<ITestRunsLocator> TestRuns();
        public abstract Task<IUser> User(UserId id);
        public abstract Task<IUser> User(string userName);
        public abstract Task<IUserLocator> Users();
        public abstract Task<IVcsRoot> VcsRoot(VcsRootId id);
        public abstract Task<IVcsRootLocator> VcsRoots();
        public abstract TeamCity WithLogResponses();
        public abstract TeamCity WithTimeout(long timeout, TimeUnit unit);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TeamCityInstance()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(false disposing);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}