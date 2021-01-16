using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Api
{
    public interface ILocator<T> where T : IIdentifiable
    {
        Task<T> ById(string stringId);
        Task<T> ById(Id id);
        IAsyncEnumerable<T> All();
    }
}