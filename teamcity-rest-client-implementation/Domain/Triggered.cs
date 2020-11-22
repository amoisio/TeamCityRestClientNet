using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

using Nito.AsyncEx;

namespace TeamCityRestClientNet.Domain
{
    class Triggered : ITriggeredInfo
    {
        public Triggered(TriggeredDto dto, TeamCityInstance instance)
        {
            this.User = new AsyncLazy<IUser>(async () 
                => await Domain.User.Create(dto.User.Id, instance).ConfigureAwait(false));
            this.Build = new AsyncLazy<IBuild>(async () 
                => await Domain.Build.Create(dto.Build.Id, instance).ConfigureAwait(false));
        }

        public AsyncLazy<IUser> User { get; }
        public AsyncLazy<IBuild> Build { get; }
    }
}