using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IUser : IIdentifiable
    {
        string Username { get; }
        string Email { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl();
    }

    public interface IInfo
    {
        AsyncLazy<IUser> User { get; }
        DateTimeOffset Timestamp { get; }
        string Text { get; }
    }

    public interface IUserLocator : ILocator<IUser>
    {
        Task<IUser> ByUsername(string userName);
    }
}