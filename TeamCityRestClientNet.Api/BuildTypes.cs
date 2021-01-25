using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IBuildType : IIdentifiable 
    {
        Id ProjectId { get; }
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
        Id InitiatedBuildTypeId { get; }
        bool AfterSuccessfulBuildOnly { get; }
        HashSet<string> IncludedBranchPatterns { get; }
        HashSet<string> ExcludedBranchPatterns { get; }
    }

    public interface IBuildTypeLocator : ILocator<IBuildType> { }
}