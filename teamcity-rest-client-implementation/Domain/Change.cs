using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Tools;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Domain
{
    class Change : Base<ChangeDto>, IChange
    {
        private Change(ChangeDto fullDto, TeamCityInstance instance)
            : base(fullDto, instance)
        {
            this.User = new AsyncLazy<IUser>(async ()
                => await Domain.User.Create(IdString, Instance).ConfigureAwait(false));
        }

        public static async Task<IChange> Create(ChangeDto dto, bool isFullDto, TeamCityInstance instance)
        {
            var fullDto = isFullDto
                ? dto
                : await instance.Service.Change(dto.Id).ConfigureAwait(false);
            return new Change(dto, instance);
        }

        public ChangeId Id => new ChangeId(IdString);
        public string Version => this.Dto.Version.SelfOrNullRef();
        public string Username => this.Dto.Username.SelfOrNullRef();
        public AsyncLazy<IUser> User { get; }
        public DateTimeOffset DateTime 
            => Utilities.ParseTeamCity(this.Dto.Date.SelfOrNullRef()).Value;
        public string Comment => this.Dto.Comment.SelfOrNullRef();
        public IVcsRootInstance VcsRootInstance 
            => this.Dto.VcsRootInstance
              .Let(rootDto => new VcsRootInstance(rootDto));

        public async Task<List<IBuild>> FirstBuilds()
        {
            var change = await Service.ChangeFirstBuilds(this.Id.stringId).ConfigureAwait(false);
            var tasks = change.Build.Select(build => Build.Create(build.Id, Instance));
            return (await Task.WhenAll(tasks).ConfigureAwait(false)).ToList();
        }

        public string GetHomeUrl(BuildConfigurationId? specificBuildConfigurationId = null, bool? includePersonalBuilds = null)
            => Instance.GetUserUrlPage(
                "viewModification.html",
                modId: Id,
                buildTypeId: specificBuildConfigurationId,
                personal: includePersonalBuilds);

        public override string ToString()
            => $"Change(id={Id}, version={Version}, username={Username}, user={User}, date={DateTime}, comment={Comment}, vcsRootInstance={VcsRootInstance})";
    }
}