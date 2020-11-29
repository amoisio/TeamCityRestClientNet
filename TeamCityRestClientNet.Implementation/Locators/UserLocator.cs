using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Locators
{
    class UserLocator : IUserLocator
    {
        private UserId? _id;
        private string _username;
        private readonly TeamCityServer _instance;

        public UserLocator(TeamCityServer instance)
        {
            _instance = instance;
        }

        public IUserLocator WithId(UserId id)
        {
            _id = id;
            return this;
        }

        public IUserLocator WithUsername(string name)
        {
            _username = name;
            return this;
        }

        public async IAsyncEnumerable<IUser> All()
        {
            if (_id.HasValue && !String.IsNullOrEmpty(_username))
            {
                throw new ArgumentException("UserLocator accepts only id or username, not both");
            }

            var locator = _id.HasValue ? $"id:{_id.Value.stringId }"
                : !String.IsNullOrEmpty(_username) ? $"username:{ _username }"
                : String.Empty;

            if (!_id.HasValue && String.IsNullOrEmpty(_username))
            {
                var users = await _instance.Service.Users().ConfigureAwait(false);
                foreach (var dto in users.User)
                {
                    yield return await Domain.User.Create(dto, false, _instance).ConfigureAwait(false);
                }
            }
            else
            {
                var dto = await _instance.Service.Users(locator).ConfigureAwait(false);
                yield return await Domain.User.Create(dto, true, _instance).ConfigureAwait(false);
            }
        }

        public async Task<List<IUser>> ToList()
        {
            var users = new List<IUser>();
            await foreach (var user in All().ConfigureAwait(false))
            {
                users.Add(user);
            }
            return users;
        }
    }
}