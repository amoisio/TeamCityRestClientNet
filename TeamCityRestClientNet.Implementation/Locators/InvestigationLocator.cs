using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    class InvestigationLocator : Locator<IInvestigation>, IInvestigationLocator
    {
        private int? _limitResult;
        private InvestigationTargetType? _targetType;
        private Id? _affectedProjectId;

        public InvestigationLocator(TeamCityServer instance) : base(instance) { }

        public override async Task<IInvestigation> ById(Id id) 
            => await Domain.Investigation.Create(id.StringId, Instance).ConfigureAwait(false);

        public override IAsyncEnumerable<IInvestigation> All()
        {
            string investigationLocator = null;
            var parameters = new List<string>();
            if (_limitResult.HasValue) parameters.Add($"count:{_limitResult}");
            if (_affectedProjectId.HasValue) parameters.Add($"affectedProject:{_affectedProjectId}");
            if (_targetType.HasValue) parameters.Add($"type:{_targetType}");
            if (parameters.IsNotEmpty())
            {
                investigationLocator = string.Join(",", parameters);
                // LOG.debug("Retrieving investigations from ${instance.serverUrl} using query '$investigationLocator'")
            }

            return new Paged2<IInvestigation, InvestigationDto, InvestigationListDto>(
                Instance,
                () => Service.Investigations(investigationLocator),
                (dto) => Domain.Investigation.Create(dto, true, Instance)
            );
        }

        public IInvestigationLocator ForProject(Id projectId)
        {
            this._affectedProjectId = projectId;
            return this;
        }

        public IInvestigationLocator LimitResults(int count)
        {
            this._limitResult = count;
            return this;
        }

        public IInvestigationLocator WithTargetType(InvestigationTargetType targetType)
        {
            this._targetType = targetType;
            return this;
        }
    }
}