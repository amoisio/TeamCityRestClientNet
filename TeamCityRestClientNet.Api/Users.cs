using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public struct UserId
    {
        public UserId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface IUser
    {
        UserId Id { get; }
        string Username { get; }
        string Name { get; }
        string Email { get; }
        /**
         * Web UI URL for user, especially useful for error and log messages
         */
        string GetHomeUrl();
    }

    public interface IUserLocator
    {
        IAsyncEnumerable<IUser> All();

        [Obsolete("use instance.user(id)")]
        IUserLocator WithId(UserId id);

        [Obsolete("use instance.user(userName)")]
        IUserLocator WithUsername(string name);

        [Obsolete("Use all() method which returns lazy sequence. Replace with all().toList().")]
        Task<List<IUser>> ToList();
    }

    public interface IInfo
    {
        AsyncLazy<IUser> User { get; }
        DateTimeOffset Timestamp { get; }
        string Text { get; }
    }
}