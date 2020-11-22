using System;
using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class BuildCommentInfo : IBuildCommentInfo
    {
        private readonly BuildCommentDto _dto;

        public BuildCommentInfo(BuildCommentDto dto, TeamCityInstance instance)
        {
            this._dto = dto;
            this.User = new AsyncLazy<IUser>(async ()
                => {
                    var user = (dto.User != null)
                       ? await Domain.User.Create(dto.User.Id, instance).ConfigureAwait(false)
                        : null;
                    if (user != null)
                        _username = user.Name;
                    return user;
                });
        }

        private string _username;
        public AsyncLazy<IUser> User { get; }
        public DateTimeOffset Timestamp 
            => Utilities.ParseTeamCity(this._dto.Timestamp)
            ?? throw new NullReferenceException();
        public string Text => this._dto.Text ?? String.Empty;

        public override string ToString()
        {
            return $"BuildCommentInfo(timestamp={Timestamp},user={_username},text={Text})";
        }
    }
}