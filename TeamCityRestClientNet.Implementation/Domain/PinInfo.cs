using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Tools;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Domain
{
    class PinInfo : IPinInfo
    {
        private readonly PinInfoDto _dto;
        public PinInfo(PinInfoDto dto, TeamCityServer instance)
        {
            _dto = dto;
            this.User = new AsyncLazy<IUser>(async () 
                => await Domain.User.Create(dto.User.Id, instance).ConfigureAwait(false));
        }

        public AsyncLazy<IUser> User { get; }
        public DateTimeOffset DateTime 
            => Utilities.ParseTeamCity(_dto.Timestamp.SelfOrNullRef()).Value;
    }
}