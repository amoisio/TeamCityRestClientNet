using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Service;

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
}