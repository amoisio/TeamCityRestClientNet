using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Tools;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Domain
{
    class Change : Base<ChangeDto>, IChange
    {
        private Change(ChangeDto fullDto, TeamCityServer instance)
            : base(fullDto, instance)
        {
            this.User = new AsyncLazy<IUser>(async ()
                => await Domain.User.Create(IdString, Instance).ConfigureAwait(false));
        }

        public static async Task<IChange> Create(ChangeDto dto, bool isFullDto, TeamCityServer instance)
        {
            var fullDto = isFullDto
                ? dto
                : await instance.Service.Change(dto.Id).ConfigureAwait(false);
            return new Change(fullDto, instance);
        }

        public Id Id => new Id(IdString);
        public string Version => this.Dto.Version.SelfOrNullRef();
        public string Username => this.Dto.Username.SelfOrNullRef();
        public AsyncLazy<IUser> User { get; }
        public DateTimeOffset DateTime 
            => Utilities.ParseTeamCity(this.Dto.Date.SelfOrNullRef()).Value;
        public string Comment => this.Dto.Comment.SelfOrNullRef();
        public IVcsRootInstance VcsRootInstance 
            => this.Dto.VcsRootInstance
              .Let(rootDto => new VcsRootInstance(rootDto));

        public string GetHomeUrl(Id? specificBuildTypeId = null, bool? includePersonalBuilds = null)
            => Instance.GetUserUrlPage(
                "viewModification.html",
                modId: Id,
                buildTypeId: specificBuildTypeId,
                personal: includePersonalBuilds);

        public override string ToString()
            => $"Change(id={Id}, version={Version}, username={Username}, user={User}, date={DateTime}, comment={Comment}, vcsRootInstance={VcsRootInstance})";
    }
}