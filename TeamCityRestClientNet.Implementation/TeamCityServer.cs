using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Locators;
using TeamCityRestClientNet.Service;
using Microsoft.Extensions.Logging.Abstractions;

namespace TeamCityRestClientNet
{
    class TeamCityServer : TeamCity
    {
        public const string TEAMCITY_DATETIME_FORMAT = "yyyyMMdd'T'HHmmssZ";
        public const string TEAMCITY_DEFAUL_LOCALE = "en-US";
        private readonly Lazy<ITeamCityService> _service;
        public ITeamCityService Service => _service.Value;
        private readonly ILogger _logger;
        internal TeamCityServer(TeamCityServiceBuilder serviceBuilder, ILogger logger = null)
        {
            if (serviceBuilder == null)
                throw new ArgumentNullException(nameof(serviceBuilder));

            this.ServerUrl = serviceBuilder.ServerUrl;
            this.ServerUrlBase = serviceBuilder.ServerUrlBase;
            this._service = new Lazy<ITeamCityService>(() => serviceBuilder.Build());
            this._logger = logger ?? NullLogger.Instance; 
        }

        public string ServerUrl { get; }

        public string ServerUrlBase { get; }

        public override async Task<IBuild> Build(BuildId id)
            => await Domain.Build.Create(id.stringId, this).ConfigureAwait(false);

        public override async Task<IBuild> Build(BuildConfigurationId buildConfigurationId, string number)
            => await new BuildLocator(this)
            .FromConfiguration(buildConfigurationId)
            .WithNumber(number)
            .Latest()
            .ConfigureAwait(false);

        public override IBuildAgentLocator BuildAgents => new BuildAgentLocator(this);

        public override IBuildAgentPoolLocator BuildAgentPools => new BuildAgentPoolLocator(this);

        public override async Task<IBuildConfiguration> BuildConfiguration(string id)
            => await Domain.BuildConfiguration.Create(id, this).ConfigureAwait(false);

        public override async Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id)
            => await BuildConfiguration(id.stringId).ConfigureAwait(false);

        public override IBuildQueue BuildQueue => new BuildQueue(this);

        public override IBuildLocator Builds => new BuildLocator(this);
        public override async Task<IChange> Change(BuildConfigurationId buildConfigurationId, string vcsRevision)
        {
            var dto = await Service.Change(buildConfigurationId.stringId, vcsRevision).ConfigureAwait(false);
            return await Domain.Change.Create(dto, true, this).ConfigureAwait(false);
        }

        public override async Task<IChange> Change(ChangeId id)
            => await Domain.Change.Create(new ChangeDto { Id = id.stringId }, false, this).ConfigureAwait(false);

        public override IInvestigationLocator Investigations
            => new InvestigationLocator(this);

        public override async Task<IProject> Project(ProjectId id)
            => await Domain.Project.Create(new ProjectDto { Id = id.stringId }, false, this).ConfigureAwait(false);

        public override IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId projectId)
            => new BuildQueue(this).QueuedBuilds(projectId);

        public override async Task<IProject> RootProject()
            => await Project(new ProjectId("_Root")).ConfigureAwait(false);

        public override ITestRunsLocator TestRuns => new TestRunsLocator(this);

        public override async Task<IUser> User(UserId id)
            => await Domain.User.Create(id.stringId, this).ConfigureAwait(false);

        public override async Task<IUser> User(string userName)
        {
            var fullDto = await Service.Users($"username:{userName}").ConfigureAwait(false);
            return await Domain.User.Create(fullDto, true, this).ConfigureAwait(false);
        }

        public override async IAsyncEnumerable<IUser> Users()
        {
            var users = await Service.Users().ConfigureAwait(false);
            foreach (var dto in users.User)
            {
                yield return await Domain.User.Create(dto, false, this).ConfigureAwait(false);
            }
        }

        public override async Task<IVcsRoot> VcsRoot(VcsRootId id)
        {
            var fullDto = await Service.VcsRoot(id.stringId).ConfigureAwait(false);
            return await Domain.VcsRoot.Create(fullDto, true, this).ConfigureAwait(false);
        }

        public override IVcsRootLocator VcsRoots => new VcsRootLocator(this);

        internal string GetUserUrlPage(
            string pageName,
            string tab = null,
            ProjectId? projectId = null,
            BuildId? buildId = null,
            TestId? testNameId = null,
            UserId? userId = null,
            ChangeId? modId = null,
            bool? personal = null,
            BuildConfigurationId? buildTypeId = null,
            string branch = null)
        {
            var param = new List<string>();

            if (tab != null)
                param.Add($"tab={WebUtility.UrlEncode(tab)}");
            if (projectId.HasValue)
                param.Add($"projectId={WebUtility.UrlEncode(projectId.Value.stringId)}");
            if (buildId.HasValue)
                param.Add($"buildId={WebUtility.UrlEncode(buildId.Value.stringId)}");
            if (testNameId.HasValue)
                param.Add($"testNameId={WebUtility.UrlEncode(testNameId.Value.stringId)}");
            if (userId.HasValue)
                param.Add($"userId={WebUtility.UrlEncode(userId.Value.stringId)}");
            if (modId.HasValue)
                param.Add($"modId={WebUtility.UrlEncode(modId.Value.stringId)}");
            if (personal.HasValue)
            {
                var personalStr = personal.Value ? "true" : "false";
                param.Add($"personal={personalStr}");
            }
            if (buildTypeId.HasValue)
                param.Add($"buildTypeId={WebUtility.UrlEncode(buildTypeId.Value.stringId)}");
            if (!String.IsNullOrEmpty(branch))
                param.Add($"branch={WebUtility.UrlEncode(branch)}");

            var paramStr = param.IsNotEmpty()
                ? String.Join("&", param)
                : String.Empty;

            return $"{this.ServerUrl}/{pageName}?{paramStr}";
        }
    }

    public class TeamCityQueryException : TeamCityException
    {
        public TeamCityQueryException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }

    public class TeamCityConversationException : TeamCityException
    {
        public TeamCityConversationException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }
}