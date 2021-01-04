using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;
using System.Linq;

namespace TeamCityRestClientNet.Domain
{
    class BuildQueue : IBuildQueue
    {
        private readonly TeamCityServer _instance;

        public BuildQueue(TeamCityServer instance)
        {
            this._instance = instance;
        }

        public IAsyncEnumerable<IBuild> QueuedBuilds(Id? projectId = null)
        {
            var parameters = projectId.HasValue
                ? new List<string>{ $"project:{projectId.Value}" }
                : new List<string>();

            var sequence = new Paged<IBuild, BuildListDto>(
                _instance,
                async () => {
                    var buildLocator = parameters.IsNotEmpty()
                        ? string.Join(",", parameters)
                        : null;
                    // LOG.debug("Retrieving queued builds from ${instance.serverUrl} using query '$buildLocator'")
                    return await _instance.Service.QueuedBuilds(buildLocator).ConfigureAwait(false);
                }, 
                async (list) => {
                    var tasks = list.Build.Select(build => Build.Create(build.Id, _instance));
                    var builds = await Task.WhenAll(tasks).ConfigureAwait(false);
                    return new Page<IBuild>(builds, list.NextHref);
                });
            return sequence;
        }

        public async Task RemoveBuild(BuildId id, string comment = "", bool reAddIntoQueue = false)
        {
            var request = new BuildCancelRequestDto
            {
                Comment = comment,
                ReaddIntoQueue = reAddIntoQueue
            };
            await _instance.Service.RemoveQueuedBuild(id.stringId, request).ConfigureAwait(false);
        }
    }
}