using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Locators
{
    class UserLocator : IUserLocator
    {
        private readonly TeamCityServer _instance;

        public UserLocator(TeamCityServer instance)
        {
            _instance = instance;
        }

        public async IAsyncEnumerable<IUser> All()
        {
            var users = await _instance.Service.Users().ConfigureAwait(false);
            foreach (var dto in users.User)
            {
                yield return await Domain.User.Create(dto, false, _instance).ConfigureAwait(false);
            }
        }
    }
}