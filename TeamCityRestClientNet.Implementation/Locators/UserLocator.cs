using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class UserLocator : Locator<IUser>, IUserLocator
    {
        public UserLocator(TeamCityServer instance) : base(instance) { }

        // <summary>
        /// Retrieve a user from TeamCity by user id.
        /// </summary>
        /// <param name="id">Id of the user to retrieve.</param>
        /// <returns>Matching user. Throws a Refit.ApiException if user not found.</returns>
        public override async Task<IUser> ById(Id id)
        {
            // _logger.LogDebug($"Retrieving user id:{id}.");
            return await Domain.User.Create(id.StringId, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a user from TeamCity by username.
        /// </summary>
        /// <param name="username">Username of the user to retrieve.</param>
        /// <returns>Matching user. Throws a Refit.ApiException if user not found.</returns>
        public async Task<IUser> ByUsername(string username)
        {
            // _logger.LogDebug($"Retrieving user username:{username}.");
            var fullDto = await Service.Users($"username:{username}").ConfigureAwait(false);
            return await Domain.User.Create(fullDto, true, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all users from TeamCity.
        /// </summary>
        /// <returns>All users defined in TeamCity.</returns>
        public override async IAsyncEnumerable<IUser> All(string initialLocator = null)
        {
            // _logger.LogDebug("Retrieving users.");
            var userListDto = await Service.Users().ConfigureAwait(false);
            foreach (var dto in userListDto.Items)
            {
                yield return await Domain.User.Create(dto, false, Instance).ConfigureAwait(false);
            }
        }
    }
}