using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Locators
{
    class InvestigationLocator : Locator, IInvestigationLocator
    {
        private int? limitResult;
        private InvestigationTargetType? targetType;
        private ProjectId? affectedProjectId;

        public InvestigationLocator(TeamCityInstance instance) : base(instance) 
        {

        }

        public async Task<IEnumerable<IInvestigation>> All()
        {
            string investigationLocator = null;
            var parameters = new List<string>();
            if (limitResult.HasValue) parameters.Add($"count:{limitResult}");
            if (affectedProjectId.HasValue) parameters.Add($"affectedProject:{affectedProjectId}");
            if (targetType.HasValue) parameters.Add($"type:{targetType}");

            if (parameters.IsNotEmpty())
            {
                investigationLocator = string.Join(",", parameters);
                // LOG.debug("Retrieving investigations from ${instance.serverUrl} using query '$investigationLocator'")
            }

            var investigations = await Service.Investigations(investigationLocator).ConfigureAwait(false);
            var tasks = investigations.Investigation
                .Select(inv => Investigation.Create(inv, true, Instance));

            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public IInvestigationLocator ForProject(ProjectId projectId)
        {
            this.affectedProjectId = projectId;
            return this;
        }

        public IInvestigationLocator LimitResults(int count)
        {
            this.limitResult = count;
            return this;
        }

        public IInvestigationLocator WithTargetType(InvestigationTargetType targetType)
        {
            this.targetType = targetType;
            return this;
        }
    }
}