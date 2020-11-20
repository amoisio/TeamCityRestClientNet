using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class PinInfo : IPinInfo
    {
        private readonly PinInfoDto _dto;
        private readonly TeamCityInstance _instance;

        public PinInfo(PinInfoDto dto, TeamCityInstance instance)
        {
            this._dto = dto;
            this._instance = instance;
        }

        public IUser User 
            => new User(this._dto.User.SelfOrNullRefException(), false, this._instance);

        public DateTimeOffset DateTime 
            => Utilities.ParseTeamCity(this._dto.Timestamp.SelfOrNullRefException()).Value;
    }
}