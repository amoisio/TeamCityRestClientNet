using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Api
{
    public interface IBuildQueue
    {
        Task RemoveBuild(Id buildId, string comment = "", bool reAddIntoQueue = false);
        IAsyncEnumerable<IBuild> All(Id? projectId = null);
    }
}