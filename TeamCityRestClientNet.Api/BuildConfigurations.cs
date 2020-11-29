using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public struct BuildConfigurationId
    {
        public BuildConfigurationId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface IBuildConfiguration
    {
        BuildConfigurationId Id { get; }
        string Name { get; }
        ProjectId ProjectId { get; }
        bool Paused { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl(string branch = null);
        AsyncLazy<List<string>> BuildTags { get; }
        AsyncLazy<List<IFinishBuildTrigger>> FinishBuildTriggers { get; }
        AsyncLazy<List<IArtifactDependency>> ArtifactDependencies { get; }
        int BuildCounter { get; }
        Task SetBuildCounter(int count);
        string BuildNumberFormat { get; }
        Task SetBuildNumberFormat(string format);
        Task SetParameter<T>(string name, T value);
        Task<IBuild> RunBuild(
            IDictionary<string, string> parameters = null,
            bool queueAtTop = false,
            bool? cleanSources = null,
            bool rebuildAllDependencies = false,
            string comment = null,
            string logicalBranchName = null,
            bool personal = false);
    }

    public interface IFinishBuildTrigger
    {
        BuildConfigurationId InitiatedBuildConfiguration { get; }
        bool AfterSuccessfulBuildOnly { get; }
        HashSet<string> IncludedBranchPatterns { get; }
        HashSet<string> ExcludedBranchPatterns { get; }
    }
}