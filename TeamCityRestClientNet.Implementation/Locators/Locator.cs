using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Locators
{
    abstract class Locator 
    {
        public Locator(TeamCityServer instance)
        {
            this.Instance = instance;
        }

        public TeamCityServer Instance { get; }
        public ITeamCityService Service => Instance.Service; 
    }

    abstract class Locator<T> : Locator, ILocator<T> where T : IIdentifiable
    {
        protected Locator(TeamCityServer instance) : base(instance) { }
        public virtual Task<T> ById(string stringId) => ById(new Id(stringId));
        public abstract Task<T> ById(Id id);
        public abstract IAsyncEnumerable<T> All();
    }
}