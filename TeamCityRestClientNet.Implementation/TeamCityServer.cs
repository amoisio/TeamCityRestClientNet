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
        // TODO: Make configurable?
        public const string TEAMCITY_DATETIME_FORMAT = "yyyyMMddTHHmmsszz00";
        public const string TEAMCITY_DEFAUL_LOCALE = "en-US";
        public ITeamCityService Service { get; }
        public ILogger Logger { get; }

        internal TeamCityServer(
            string serverUrl,
            string serverUrlBase,
            ITeamCityService service,
            ILogger logger)
        {
            if (String.IsNullOrWhiteSpace(serverUrl))
                throw new ArgumentNullException(nameof(serverUrl));

            this.ServerUrl = serverUrl;

            if (String.IsNullOrWhiteSpace(serverUrlBase))
                this.ServerUrlBase = string.Empty;
            else 
                this.ServerUrlBase = serverUrlBase;

            this.Service = service ?? throw new ArgumentNullException(nameof(service));
            this.Logger = logger ?? NullLogger.Instance;
        }

        public string ServerUrl { get; }

        public string ServerUrlBase { get; }

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

        public override IBuildTypeLocator BuildTypes => new BuildTypeLocator(this);
       
        public override IBuildQueue BuildQueue => new BuildQueue(this);

        /// <summary>
        /// Retrieve build locator.
        /// </summary>
        /// <returns>Locator used for interacting with builds.</returns>
        public override IBuildLocator Builds => new BuildLocator(this);
        public override IChangeLocator Changes => new ChangeLocator(this);
        public override IInvestigationLocator Investigations => new InvestigationLocator(this);
        public override IProjectLocator Projects => new ProjectLocator(this);
        public override ITestRunsLocator TestRuns => new TestRunsLocator(this);
        public override IUserLocator Users => new UserLocator(this);
        public override IVcsRootLocator VcsRoots => new VcsRootLocator(this);

        internal string GetUserUrlPage(
            string pageName,
            string tab = null,
            Id? projectId = null,
            Id? buildId = null,
            Id? testNameId = null,
            Id? userId = null,
            Id? modId = null,
            bool? personal = null,
            Id? buildTypeId = null,
            string branch = null)
        {
            var param = new List<string>();

            if (tab != null)
                param.Add($"tab={WebUtility.UrlEncode(tab)}");
            if (projectId.HasValue)
                param.Add($"projectId={WebUtility.UrlEncode(projectId.Value.StringId)}");
            if (buildId.HasValue)
                param.Add($"buildId={WebUtility.UrlEncode(buildId.Value.StringId)}");
            if (testNameId.HasValue)
                param.Add($"testNameId={WebUtility.UrlEncode(testNameId.Value.StringId)}");
            if (userId.HasValue)
                param.Add($"userId={WebUtility.UrlEncode(userId.Value.StringId)}");
            if (modId.HasValue)
                param.Add($"modId={WebUtility.UrlEncode(modId.Value.StringId)}");
            if (personal.HasValue)
            {
                var personalStr = personal.Value ? "true" : "false";
                param.Add($"personal={personalStr}");
            }
            if (buildTypeId.HasValue)
                param.Add($"buildTypeId={WebUtility.UrlEncode(buildTypeId.Value.StringId)}");
            if (!String.IsNullOrEmpty(branch))
                param.Add($"branch={WebUtility.UrlEncode(branch)}");

            var paramStr = param.IsNotEmpty()
                ? String.Join("&", param)
                : String.Empty;

            return $"{this.ServerUrl}/{pageName}?{paramStr}";
        }
    }
}