using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nito.AsyncEx;

using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class BuildAgent : Base<BuildAgentDto>, IBuildAgent
    {
        private BuildAgent(BuildAgentDto fullDto, TeamCityInstance instance) 
            : base(fullDto, instance)
        {
            this.Pool = new AsyncLazy<IBuildAgentPool>(async ()
                => await BuildAgentPool.Create(this.Dto.Pool.Id, Instance).ConfigureAwait(false));

            this.CurrentBuild = new AsyncLazy<IBuild>(async ()
                => String.IsNullOrEmpty(this.Dto.Build.Id)
                    ? null
                    : await Build.Create(this.Dto.Build.Id, Instance).ConfigureAwait(false));
        }

        public static async Task<IBuildAgent> Create(string idString, TeamCityInstance instance)
        {
            var dto = await instance.Service.Agents($"id:{idString}").ConfigureAwait(false);
            return new BuildAgent(dto, instance);
        }

        public BuildAgentId Id => new BuildAgentId(IdString);
        public string Name => Dto.Name.SelfOrNullRef();
        public AsyncLazy<IBuildAgentPool> Pool { get; }
        public bool Connected => Dto.Connected.Value;
        public bool Enabled => Dto.Enabled.Value;
        public bool Authorized => Dto.Authorized.Value;
        public bool Outdated => !Dto.Uptodate.Value;
        public string IpAddress => Dto.Ip.SelfOrNullRef();
        public List<IParameter> Parameters
            => Dto.Properties
                ?.Property
                .Select(prop => new Parameter(prop))
                .ToList<IParameter>();
        public IInfo EnabledInfo 
            => this.Dto.EnabledInfo
                .Let(info => info.Comment
                    .Let(comment => new BuildAgentInfo(
                        comment.User.Id,
                        Utilities.ParseTeamCity(comment.Timestamp).Value,
                        comment.Text ?? String.Empty,
                        Instance)
                    )
                );
        public IInfo AuthorizedInfo
            => this.Dto.AuthorizedInfo
                .Let(info => info.Comment
                    .Let(comment => new BuildAgentInfo(
                        comment.User.Id,
                        Utilities.ParseTeamCity(comment.Timestamp).Value,
                        comment.Text ?? String.Empty,
                        Instance)
                    )
                );
        public AsyncLazy<IBuild> CurrentBuild { get; }
        public string GetHomeUrl()
            => $"{Instance.ServerUrl}/agentDetails.html?id={Id.stringId}";

        public override string ToString() => $"BuildAgent(id={Id}, name={Name})";

        private class BuildAgentInfo : IInfo
        {
            public BuildAgentInfo(string userId, DateTimeOffset timestamp, string text, TeamCityInstance instance)
            {
                this.User = new AsyncLazy<IUser>(async () 
                    => await Domain.User.Create(userId, instance).ConfigureAwait(false));
                this.Timestamp = timestamp;
                this.Text = text;
            }

            public AsyncLazy<IUser> User { get; }
            public DateTimeOffset Timestamp { get; }
            public string Text { get; }
        }
    }
}