using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Api
{
    public interface IBuildQueue
    {
        Task RemoveBuild(BuildId id, string comment = "", bool reAddIntoQueue = false);
        IAsyncEnumerable<IBuild> QueuedBuilds(Id? projectId = null);
    }
}