using System;
using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    internal class BuildCanceledInfo : IBuildCanceledInfo
    {
        private readonly BuildCanceledDto _dto;
        internal BuildCanceledInfo(BuildCanceledDto dto, TeamCityInstance instance)
        {
            this._dto = dto;
            this.User = new AsyncLazy<IUser>(async ()
                => dto.User != null
                    ? await Domain.User.Create(dto.User.Id, instance).ConfigureAwait(false)
                    : null);
        }

        public AsyncLazy<IUser> User { get; }
        /*CancelDateTime*/
        public DateTimeOffset Timestamp 
            => Utilities.ParseTeamCity(_dto.Timestamp)
            ?? throw new NullReferenceException();
        public string Text => _dto.Text ?? String.Empty;
    }
}