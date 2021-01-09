using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tools;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class BuildLocator : Locator<IBuild>, IBuildLocator
    {
        private Id? _buildTypeId;
        private Id? _snapshotDependencyTo;
        private string _number;
        private string _vcsRevision;
        private DateTimeOffset? _since;
        private DateTimeOffset? _until;
        private BuildStatus? _status = BuildStatus.SUCCESS;
        private List<string> _tags = new List<string>();
        private int? _limitResults;
        private int? _pageSize;
        private string _branch;
        private bool _includeAllBranches = false;
        private bool _pinnedOnly = false;
        private string _personal;
        private string _running;
        private string _canceled;

        public BuildLocator(TeamCityServer instance) : base(instance) { }

        private DateTimeOffset? SinceUTC => _since?.ToUniversalTime();
        private string TeamCitySince => SinceUTC?.ToString(TeamCityServer.TEAMCITY_DATETIME_FORMAT);

        private DateTimeOffset? UntilUTC => _until?.ToUniversalTime();
        private string TeamCityUntil => UntilUTC?.ToString(TeamCityServer.TEAMCITY_DATETIME_FORMAT);

        public IBuildLocator FromBuildType(Id buildTypeId)
        {
            _buildTypeId = buildTypeId;
            return this;
        }

        public IBuildLocator SnapshotDependencyTo(Id buildId)
        {
            _snapshotDependencyTo = buildId;
            return this;
        }

        public IBuildLocator WithNumber(string buildNumber)
        {
            _number = buildNumber;
            return this;
        }

        public IBuildLocator WithVcsRevision(string vcsRevision)
        {
            _vcsRevision = vcsRevision;
            return this;
        }

        public IBuildLocator IncludeFailed()
        {
            _status = null;
            return this;
        }

        public IBuildLocator WithStatus(BuildStatus status)
        {
            _status = status;
            return this;
        }

        public IBuildLocator IncludeRunning()
        {
            _running = "any";
            return this;
        }

        public IBuildLocator OnlyRunning()
        {
            _running = "true";
            return this;
        }

        public IBuildLocator IncludeCanceled()
        {
            _canceled = "any";
            return this;
        }

        public IBuildLocator OnlyCanceled()
        {
            _canceled = "true";
            return this;
        }

        public IBuildLocator WithTag(string tag)
        {
            _tags.Add(tag);
            return this;
        }

        public IBuildLocator WithBranch(string branch)
        {
            _branch = branch;
            return this;
        }

        public IBuildLocator Since(DateTimeOffset date)
        {
            _since = date;
            return this;
        }

        public IBuildLocator Until(DateTimeOffset date)
        {
            _until = date;
            return this;
        }

        public IBuildLocator WithAllBranches()
        {
            if (_branch != null)
            {
                // LOG.warn("Branch is ignored because of #withAllBranches")
            }

            _includeAllBranches = true;
            return this;
        }

        public IBuildLocator PinnedOnly()
        {
            _pinnedOnly = true;
            return this;
        }

        public IBuildLocator IncludePersonal()
        {
            _personal = "any";
            return this;
        }

        public IBuildLocator OnlyPersonal()
        {
            _personal = "true";
            return this;
        }

        public IBuildLocator LimitResults(int count)
        {
            _limitResults = count;
            return this;
        }

        public IBuildLocator PageSize(int pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        public IBuildLocator WithAnyStatus() => IncludeFailed();

        public async Task<IBuild> Latest()
        {
            return await LimitResults(1).All().FirstOrDefaultAsync();
        }

        public override async Task<IBuild> ById(Id id)
            => await Domain.Build.Create(id.StringId, Instance).ConfigureAwait(false);

        public override IAsyncEnumerable<IBuild> All(string initialLocator = null)
        {
            int? count = Utility.SelectRestApiCountForPagedRequests(_limitResults, _pageSize);

            var parameters = Utility.ListOfNotNull(
                _buildTypeId?.StringId.Let("buildType:{0}"),
                _snapshotDependencyTo?.StringId.Let("snapshotDependency:(to:(id:{0}))"),
                _number.Let("number:{0}"),
                _running.Let("running:{0}"),
                _canceled.Let("canceled:{0}"),
                _vcsRevision.Let("revision:{0}"),
                _status.Let("status:{0}"),
                _tags.IsNotEmpty() ? "tags:(" + string.Join(",", _tags) + ")" : null,
                _pinnedOnly ? "pinned:true" : null,
                count.Let("count:{0}"),
                TeamCitySince?.Let("sinceDate:{0}"),
                TeamCityUntil?.Let("untilDate:{0}"),
                _includeAllBranches ? "branch:default:any" : _branch.Let("branch:{0}"),
                _personal?.Let("personal:{0}"),
                // Always use default filter since sometimes TC automatically switches between
                // defaultFilter:true and defaultFilter:false
                // See BuildPromotionFinder.java in rest-api, setLocatorDefaults method
                "defaultFilter:true"
            );

            if (parameters.IsEmpty())
            {
                throw new ArgumentException("At least one parameter should be specified");
            }
            var query = String.Join(",", parameters);

            var sequence = new Paged2<IBuild, BuildDto, BuildListDto>(
                Instance,
                () => Service.Builds(query),
                (dto) => Domain.Build.Create(dto.Id, Instance)
            );

            var limitResults1 = _limitResults;
            return limitResults1.HasValue
                ? sequence.Take(limitResults1.Value)
                : sequence;
        }
    }
}