using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class VcsRootLocator : Locator<IVcsRoot>, IVcsRootLocator
    {
        public VcsRootLocator(TeamCityServer instance) : base(instance) { }

        /// <summary>
        /// Retrieve a vcs root from TeamCity by id.
        /// </summary>
        /// <param name="id">Id of the vcs root to retrieve.</param>
        /// <returns>Matching vcs root. Throws a Refit.ApiException if vcs root not found.</returns>
        public override async Task<IVcsRoot> ById(Id id)
        {
            // _logger.LogDebug($"Retrieving vcs root id:{id}.");
            var fullDto = await Service.VcsRoot(id.StringId).ConfigureAwait(false);
            return await Domain.VcsRoot.Create(fullDto, true, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all vcs roots from TeamCity.
        /// </summary>
        /// <returns>All vcs roots defined in TeamCity.</returns>
        public override IAsyncEnumerable<IVcsRoot> All(string initialLocator = null)
        {
            return new Paged2<IVcsRoot, VcsRootDto, VcsRootListDto>(
                Instance,
                () => Service.VcsRoots(),
                (dto) => Domain.VcsRoot.Create(dto, false, Instance)
            );
        }
    }
}