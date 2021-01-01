using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class VcsRootLocator : Locator, IVcsRootLocator
    {
        public VcsRootLocator(TeamCityServer instance) : base(instance)
        {

        }


        /// <summary>
        /// Retrieve a vcs root from TeamCity by id.
        /// </summary>
        /// <param name="id">Id of the vcs root to retrieve.</param>
        /// <returns>Matching vcs root. Throws a Refit.ApiException if vcs root not found.</returns>
        public async Task<IVcsRoot> VcsRoot(VcsRootId id)
        {
            // _logger.LogDebug($"Retrieving vcs root id:{id}.");
            var fullDto = await Service.VcsRoot(id.stringId).ConfigureAwait(false);
            return await Domain.VcsRoot.Create(fullDto, true, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all vcs roots from TeamCity.
        /// </summary>
        /// <returns>All vcs roots defined in TeamCity.</returns>
        public IAsyncEnumerable<IVcsRoot> All()
        {
            var sequence = new Paged<IVcsRoot, VcsRootListDto>(
                 Instance,
                 async () =>
                 {
                    //  _logger.LogDebug("Retrieving vcs roots.");
                     return await Service.VcsRoots().ConfigureAwait(false);
                 },
                 async (list) =>
                 {
                     var tasks = list.VcsRoot.Select(root => Domain.VcsRoot.Create(root, false, Instance));
                     var dtos = await Task.WhenAll(tasks).ConfigureAwait(false);
                     return new Page<IVcsRoot>(dtos, list.NextHref);
                 }
             );
            return sequence;
        }
    }
}