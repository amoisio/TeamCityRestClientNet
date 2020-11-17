using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Implementations
{
    class BuildCommentInfo : IBuildCommentInfo
    {
        private readonly BuildCommentDto _dto;
        private readonly TeamCityInstance _instance;

        public BuildCommentInfo(BuildCommentDto dto, TeamCityInstance instance)
        {
            this._dto = dto;
            this._instance = instance;
        }

        public IUser User 
            => this._dto.User != null
                ? new User(this._dto.User, false, this._instance)
                : null;

        public DateTimeOffset Timestamp 
            => Utilities.ParseTeamCity(this._dto.Timestamp)
            ?? throw new NullReferenceException();

        public string Text => this._dto.Text ?? String.Empty;

        public override string ToString()
        {
            return $"BuildCommentInfo(timestamp={Timestamp},user={User?.Username},text={Text})";
        }
    }
}