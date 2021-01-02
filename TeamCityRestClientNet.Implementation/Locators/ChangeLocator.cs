using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class ChangeLocator : Locator, IChangeLocator
    {
        public ChangeLocator(TeamCityServer instance) : base(instance)
        {

        }

        public IAsyncEnumerable<IChange> All()
        {
            var sequence = new Paged<IChange, ChangesDto>(
                 Instance,
                 async () =>
                 {
                    //  _logger.LogDebug("Retrieving changes.");
                     return await Service.Changes(null, null).ConfigureAwait(false);
                 },
                 async (list) =>
                 {
                     var tasks = list.Items.Select(change => Domain.Change.Create(change, false, Instance));
                     var dtos = await Task.WhenAll(tasks).ConfigureAwait(false);
                     return new Page<IChange>(dtos, list.NextHref);
                 }
             );
            return sequence;
        }

        // TODO: This seems suspect...
        public async Task<IChange> Change(BuildConfigurationId buildConfigurationId, string vcsRevision)
        {
            var dto = await Service.Change(buildConfigurationId.stringId, vcsRevision).ConfigureAwait(false);
            return await Domain.Change.Create(dto, true, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a change from TeamCity by change id.
        /// </summary>
        /// <param name="id">Id of the change to retrieve.</param>
        /// <returns>Matching change. Throws a Refit.ApiException if change not found.</returns>
        public async Task<IChange> Change(ChangeId id)
            => await Domain.Change.Create(new ChangeDto { Id = id.stringId }, false, Instance).ConfigureAwait(false);
    }
}