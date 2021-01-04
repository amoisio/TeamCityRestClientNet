using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IProject : IIdentifiable
    {
        string Name { get; }
        bool Archived { get; }
        Id? ParentProjectId { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl(string branch = null);
        string GetTestHomeUrl(TestId testId);
        AsyncLazy<List<IProject>> ChildProjects { get; }
        AsyncLazy<List<IBuildConfiguration>> BuildConfigurations { get; }
        List<IParameter> Parameters { get; }
        Task SetParameter(string name, string value);
        /**
         * See properties example from existing VCS roots via inspection of the following url:
         * https://teamcity/app/rest/vcs-roots/id:YourVcsRootId
         */
        Task<IVcsRoot> CreateVcsRoot(Id id, string name, VcsRootType type, IDictionary<string, string> properties);
        Task<IProject> CreateProject(Id id, string name);
        Task Delete();
        /**
         * XML in the same format as
         * https://teamcity/app/rest/buildTypes/YourBuildConfigurationId
         * returns
         */
        Task<IBuildConfiguration> CreateBuildConfiguration(string buildConfigurationDescriptionXml);
    }

    public interface IProjectLocator : ILocator<IProject>
    {
        Task<IProject> RootProject();
    }
}