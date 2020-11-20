using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class Change : Base<ChangeDto>, IChange
    {
        public Change(ChangeDto dto, bool isFullDto, TeamCityInstance instance)
            : base(dto, isFullDto, instance)
        {
            
        }

        public ChangeId Id => new ChangeId(IdString);
        public string Version => NotNullSync(dto => dto.Version);
        public string Username => NotNullSync(dto => dto.Username);
        public IUser User 
            => NullableSync(dto => dto.User)
              .Let(userDto => new User(userDto, false, Instance));
        public DateTimeOffset DateTime 
            => Utilities.ParseTeamCity(NotNullSync(dto => dto.Date)).Value;
        public string Comment => NotNullSync(dto => dto.Comment);
        public IVcsRootInstance VcsRootInstance 
            => NullableSync(dto => dto.VcsRootInstance)
              .Let(rootDto => new VcsRootInstance(rootDto));

        public List<IBuild> FirstBuilds()
            => Service
            .ChangeFirstBuilds(this.Id.stringId)
            .GetAwaiter()
            .GetResult()
            .Build
            .Select(build => new Build(build, false, Instance))
            .ToList<IBuild>();

        public string GetHomeUrl(BuildConfigurationId? specificBuildConfigurationId = null, bool? includePersonalBuilds = null)
            => Instance.GetUserUrlPage(
                "viewModification.html",
                modId: Id,
                buildTypeId: specificBuildConfigurationId,
                personal: includePersonalBuilds);

        public override string ToString()
            => $"Change(id={Id}, version={Version}, username={Username}, user={User}, date={DateTime}, comment={Comment}, vcsRootInstance={VcsRootInstance})";

        protected override async Task<ChangeDto> FetchFullDto()
            => await Service.Change(IdString);
    }
}