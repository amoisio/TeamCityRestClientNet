using System;
using System.Collections.Generic;
using System.Linq;

using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Tools;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Implementations
{
    class BuildLocator : IBuildLocator
    {
        private readonly ITeamCityService _service;
        private readonly TeamCityInstance _instance;
        private BuildConfigurationId? _buildConfigurationId;
        private BuildId? _snapshotDependencyTo;
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

        BuildLocator(TeamCityInstance instance)
        {
            this._instance = instance;
            this._service = instance.Service;
        }


        private DateTimeOffset? SinceUTC => this._since?.ToUniversalTime();
        private string TeamCitySince => SinceUTC?.ToString(TeamCityInstance.TEAMCITY_DATETIME_FORMAT);

        private DateTimeOffset? UntilUTC => this._until?.ToUniversalTime();
        private string TeamCityUntil => UntilUTC?.ToString(TeamCityInstance.TEAMCITY_DATETIME_FORMAT);


        public IBuildLocator FromConfiguration(BuildConfigurationId buildConfigurationId)
        {
            this._buildConfigurationId = buildConfigurationId;
            return this;
        }

        public IBuildLocator SnapshotDependencyTo(BuildId buildId)
        {
            this._snapshotDependencyTo = buildId;
            return this;
        }

        public IBuildLocator WithNumber(string buildNumber)
        {
            this._number = buildNumber;
            return this;
        }

        public IBuildLocator WithVcsRevision(string vcsRevision)
        {
            this._vcsRevision = vcsRevision;
            return this;
        }

        public IBuildLocator IncludeFailed()
        {
            this._status = null;
            return this;
        }

        public IBuildLocator WithStatus(BuildStatus status)
        {
            this._status = status;
            return this;
        }

        public IBuildLocator IncludeRunning()
        {
            this._running = "any";
            return this;
        }

        public IBuildLocator OnlyRunning()
        {
            this._running = "true";
            return this;
        }

        public IBuildLocator IncludeCanceled()
        {
            this._canceled = "any";
            return this;
        }

        public IBuildLocator OnlyCanceled()
        {
            this._canceled = "true";
            return this;
        }

        public IBuildLocator WithTag(string tag)
        {
            this._tags.Add(tag);
            return this;
        }

        public IBuildLocator WithBranch(string branch)
        {
            this._branch = branch;
            return this;
        }

        public IBuildLocator Since(DateTimeOffset date)
        {
            this._since = date;
            return this;
        }

        public IBuildLocator Until(DateTimeOffset date)
        {
            this._until = date;
            return this;
        }

        public IBuildLocator WithAllBranches()
        {
            if (this._branch != null)
            {
                // LOG.warn("Branch is ignored because of #withAllBranches")
            }

            this._includeAllBranches = true;
            return this;
        }

        public IBuildLocator PinnedOnly()
        {
            this._pinnedOnly = true;
            return this;
        }

        public IBuildLocator IncludePersonal()
        {
            this._personal = "any";
            return this;
        }

        public IBuildLocator OnlyPersonal()
        {
            this._personal = "true";
            return this;
        }

        public IBuildLocator LimitResults(int count)
        {
            this._limitResults = count;
            return this;
        }

        public IBuildLocator PageSize(int pageSize)
        {
            this._pageSize = pageSize;
            return this;
        }

        public IBuildLocator WithAnyStatus() => IncludeFailed();

        public async Task<IBuild> Latest()
        {
            return await this.LimitResults(1).All().FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<IBuild> All()
        {
            int count = 0;//selectRestApiCountForPagedRequests(limitResults = limitResults, pageSize = pageSize)

            var parameters = Utilities.ListOfNotNull(
                this._buildConfigurationId?.stringId.Let("buildType:{0}"),
                this._snapshotDependencyTo?.stringId.Let("snapshotDependency:(to:(id:{0}))"),
                this._number.Let("number:{0}"),
                this._running.Let("running:{0}"),
                this._canceled.Let("canceled:{0}"),
                this._vcsRevision.Let("revision:{0}"),
                this._status.Let("status:{0}"),
                this._tags.IsNotEmpty() ? "tags:(" + string.Join(",", this._tags) + ")" : null,
                this._pinnedOnly ? "pinned:true" : null,
                count.Let("count:{0}"),
                this.TeamCitySince?.Let("sinceDate:{0}"),
                this.TeamCityUntil?.Let("untilDate:{0}"),
                !this._includeAllBranches ? this._branch?.Let("branch:{0}") : "branch:default:any",
                this._personal?.Let("personal:{0}"),
                // Always use default filter since sometimes TC automatically switches between
                // defaultFilter:true and defaultFilter:false
                // See BuildPromotionFinder.java in rest-api, setLocatorDefaults method
                "defaultFilter:true"
            );

            if (parameters.IsEmpty())
            {
                throw new ArgumentException("At least one parameter should be specified");
            }

            var sequence = new Paged<IBuild, BuildListDto>(
                this._service,
                async () =>
                {
                    var query = String.Join(",", parameters);
                    // LOG.debug("Retrieving builds from ${instance.serverUrl} using query '$IBuildLocator'")
                    return await this._service.Builds(query);
                },
                async (list) => new Page<IBuild>(
                    list.Build.Select(IdDto => new Build(IdDto, false, this._instance)).ToArray(),
                    list.NextHref)
            );

            var limitResults1 = _limitResults;
            return limitResults1.HasValue
                ? sequence.Take(limitResults1.Value)
                : sequence;
        }

        public async Task<List<IBuild>> List() 
            => await this.All().ToListAsync();
    }
}