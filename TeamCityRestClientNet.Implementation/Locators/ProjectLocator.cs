using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class ProjectLocator : Locator<IProject>, IProjectLocator
    {
        public ProjectLocator(TeamCityServer instance) : base(instance) { }

        /// <summary>
        /// Retrieve a project from TeamCity by project id.
        /// </summary>
        /// <param name="id">Id of the project to retrieve.</param>
        /// <returns>Matching project. Throws a Refit.ApiException if project not found.</returns>
        public override async Task<IProject> ById(Id id)
            => await Domain.Project.Create(new ProjectDto { Id = id.StringId }, false, Instance).ConfigureAwait(false);

        /// <summary>
        /// Retrieves the root project.
        /// </summary>
        /// <returns>The root project.</returns>
        public async Task<IProject> RootProject()
            => await ById(new Id("_Root")).ConfigureAwait(false);

        /// <summary>
        /// Retrieved all projects.
        /// </summary>
        /// <param name="initialLocator">Locator string for the first query.</param>
        /// <returns>All projects.</returns>
        public override IAsyncEnumerable<IProject> All(string initialLocator = null)
        {
            return new Paged2<IProject, ProjectDto, ProjectListDto>(
                Instance,
                () => Service.Projects(),
                (dto) => Domain.Project.Create(dto, false, Instance)
            );
        }
    }
}