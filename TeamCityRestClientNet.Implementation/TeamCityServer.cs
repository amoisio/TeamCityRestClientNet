using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Locators;
using TeamCityRestClientNet.Service;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;

namespace TeamCityRestClientNet
{
    class TeamCityServer : TeamCity
    {
        // TODO: Does this need to be configurable?
        public const string TEAMCITY_DATETIME_FORMAT = "yyyyMMddTHHmmsszz00";
        public const string TEAMCITY_DEFAUL_LOCALE = "en-US";
        public ITeamCityService Service { get; }
        private readonly ILogger _logger;

        internal TeamCityServer(
            string serverUrl,
            string serverUrlBase,
            ITeamCityService service,
            ILogger logger = null)
        {
            if (String.IsNullOrWhiteSpace(serverUrl))
                throw new ArgumentNullException(nameof(serverUrl));

            this.ServerUrl = serverUrl;

            if (String.IsNullOrWhiteSpace(serverUrlBase))
                this.ServerUrlBase = string.Empty;
            else 
                this.ServerUrlBase = serverUrlBase;

            this.Service = service ?? throw new ArgumentNullException(nameof(service));
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

        /// <summary>
        /// Retrieve build agent locator.
        /// </summary>
        /// <returns>Locator used for interacting with build agents.</returns>
        public override IBuildAgentLocator BuildAgents => new BuildAgentLocator(this);

        /// <summary>
        /// Retrieve build agent pool locator.
        /// </summary>
        /// <returns>Locator used for interacting with build agent pools.</returns>
        public override IBuildAgentPoolLocator BuildAgentPools => new BuildAgentPoolLocator(this);

        public override async Task<IBuildConfiguration> BuildConfiguration(string id)
            => await Domain.BuildConfiguration.Create(id, this).ConfigureAwait(false);

        public override async Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id)
            => await BuildConfiguration(id.stringId).ConfigureAwait(false);

        public override IBuildQueue BuildQueue => new BuildQueue(this);

        public override IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId projectId)
            => new BuildQueue(this).QueuedBuilds(projectId);

        /// <summary>
        /// Retrieve build locator.
        /// </summary>
        /// <returns>Locator used for interacting with builds.</returns>
        public override IBuildLocator Builds => new BuildLocator(this);

        public override IChangeLocator Changes => new ChangeLocator(this);

        

        // TODO: comments + tests
        public override IInvestigationLocator Investigations
            => new InvestigationLocator(this);

        /// <summary>
        /// Retrieve a project from TeamCity by project id.
        /// </summary>
        /// <param name="id">Id of the project to retrieve.</param>
        /// <returns>Matching project. Throws a Refit.ApiException if project not found.</returns>
        public override async Task<IProject> Project(ProjectId id)
            => await Domain.Project.Create(new ProjectDto { Id = id.stringId }, false, this).ConfigureAwait(false);

        /// <summary>
        /// Retrieves the root project.
        /// </summary>
        /// <returns>The root project.</returns>
        public override async Task<IProject> RootProject()
            => await Project(new ProjectId("_Root")).ConfigureAwait(false);

        // TODO: comments + tests
        public override ITestRunsLocator TestRuns => new TestRunsLocator(this);

        public override IUserLocator Users => new UserLocator(this);

        /// <summary>
        /// Retrieve a vcs root from TeamCity by id.
        /// </summary>
        /// <param name="id">Id of the vcs root to retrieve.</param>
        /// <returns>Matching vcs root. Throws a Refit.ApiException if vcs root not found.</returns>
        public override async Task<IVcsRoot> VcsRoot(VcsRootId id)
        {
            _logger.LogDebug($"Retrieving vcs root id:{id}.");
            var fullDto = await Service.VcsRoot(id.stringId).ConfigureAwait(false);
            return await Domain.VcsRoot.Create(fullDto, true, this).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all vcs roots from TeamCity.
        /// </summary>
        /// <returns>All vcs roots defined in TeamCity.</returns>
        public override IAsyncEnumerable<IVcsRoot> VcsRoots()
        {
            var sequence = new Paged<IVcsRoot, VcsRootListDto>(
                 this,
                 async () =>
                 {
                     _logger.LogDebug("Retrieving vcs roots.");
                     return await Service.VcsRoots().ConfigureAwait(false);
                 },
                 async (list) =>
                 {
                     var tasks = list.VcsRoot.Select(root => Domain.VcsRoot.Create(root, false, this));
                     var dtos = await Task.WhenAll(tasks).ConfigureAwait(false);
                     return new Page<IVcsRoot>(dtos, list.NextHref);
                 }
             );
            return sequence;
        }

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
}