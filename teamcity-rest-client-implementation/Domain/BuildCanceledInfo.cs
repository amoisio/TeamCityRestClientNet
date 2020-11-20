using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    internal class BuildCanceledInfo : IBuildCanceledInfo
    {
        private readonly BuildCanceledDto _dto;
        private readonly TeamCityInstance _instance;

        internal BuildCanceledInfo(BuildCanceledDto dto, TeamCityInstance instance)
        {
            this._dto = dto;
            this._instance = instance;
        }

        public IUser User
         => _dto.User != null
            ? new User(_dto.User, false, _instance)
            : null;

        public DateTimeOffset CancelDateTime
            => Utilities.ParseTeamCity(_dto.Timestamp)
            ?? throw new NullReferenceException();

        public string Text => _dto.Text ?? String.Empty;
    }
}