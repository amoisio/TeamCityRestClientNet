using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class BuildAgent : Base<BuildAgentDto>, IBuildAgent
    {
        internal BuildAgent(BuildAgentDto dto, bool isFullDto, TeamCityInstance instance) : base(dto, isFullDto, instance)
        {

        }

        public BuildAgentId Id => new BuildAgentId(IdString);

        public string Name => NotNull(dto => dto.Name);

        public IBuildAgentPool Pool 
            => new BuildAgentPool(NotNull(dto => dto.Pool), false, Instance);

        public bool Connected => NotNull(dto => dto.Connected).Value;

        public bool Enabled => NotNull(dto => dto.Enabled).Value;

        public bool Authorized => NotNull(dto => dto.Authorized).Value;

        public bool Outdated => !NotNull(dto => dto.Uptodate).Value;

        public string IpAddress => NotNull(dto => dto.Ip);

        public List<IParameter> Parameters
            => NotNull(dto => dto.Properties?.Property)
                .Select(prop => new Parameter(prop))
                .ToList<IParameter>();

        public IBuildAgentEnabledInfo EnabledInfo
            => this.FullDto.EnabledInfo
                .Let(info => info.Comment
                    .Let(comment => new BuildAgentEnabledInfo(
                        comment.User.Let(user => new User(user, false, Instance)),
                        Utilities.ParseTeamCity(comment.Timestamp).Value,
                        comment.Text ?? String.Empty
                    )));

        public IBuildAgentAuthorizedInfo AuthorizedInfo
            => this.FullDto.AuthorizedInfo
                .Let(info => info.Comment
                    .Let(comment => new BuildAgentAuthorizedInfo(
                        comment.User.Let(user => new User(user, false, Instance)),
                        Utilities.ParseTeamCity(comment.Timestamp).Value,
                        comment.Text ?? String.Empty
                    )));

        public IBuild CurrentBuild
            => this.FullDto.Build.Let(dto => dto.Id == null ? null : new Build(dto, false, Instance));

        public string GetHomeUrl()
            => $"{Instance.ServerUrl}/agentDetails.html?id={Id.stringId}";


        public override string ToString() => $"BuildAgent(id={Id}, name={Name})";

        protected override async Task<BuildAgentDto> FetchFullDto()
            => await Service.Agents($"id:{IdString}");

        private class BuildAgentAuthorizedInfo : IBuildAgentAuthorizedInfo
        {
            public BuildAgentAuthorizedInfo(IUser user, DateTimeOffset timestamp, string text)
            {
                this.User = user;
                this.Timestamp = timestamp;
                this.Text = text;
            }

            public IUser User { get; }

            public DateTimeOffset Timestamp { get; }

            public string Text { get; }
        }

        private class BuildAgentEnabledInfo : IBuildAgentEnabledInfo
        {
            public BuildAgentEnabledInfo(IUser user, DateTimeOffset timestamp, string text)
            {
                this.User = user;
                this.Timestamp = timestamp;
                this.Text = text;
            }

            public IUser User { get; }

            public DateTimeOffset Timestamp { get; }

            public string Text { get; }
        }
    }
}