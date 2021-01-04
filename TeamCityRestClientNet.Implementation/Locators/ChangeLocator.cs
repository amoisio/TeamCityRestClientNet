using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class ChangeLocator : Locator<IChange>, IChangeLocator
    {
        public ChangeLocator(TeamCityServer instance) : base(instance)
        {

        }
        // TODO: This seems suspect...
        public async Task<IChange> ByBuildConfigurationId(BuildConfigurationId buildConfigurationId, string vcsRevision)
        {
            var dto = await Service.Change(buildConfigurationId.stringId, vcsRevision).ConfigureAwait(false);
            return await Domain.Change.Create(dto, true, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a change from TeamCity by change id.
        /// </summary>
        /// <param name="id">Id of the change to retrieve.</param>
        /// <returns>Matching change. Throws a Refit.ApiException if change not found.</returns>
        public async override Task<IChange> ById(Id id)
            => await Domain.Change.Create(new ChangeDto { Id = id.StringId }, false, Instance).ConfigureAwait(false);

        public override IAsyncEnumerable<IChange> All(string initialLocator = null)
        {
            return new Paged2<IChange, ChangeDto, ChangeListDto>(
                Instance,
                () => Service.Changes(null, null),
                change => Domain.Change.Create(change, false, Instance)
            );
        }
    }
}