using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Locators
{
    abstract class Locator 
    {
        public Locator(TeamCityInstance instance)
        {
            this.Instance = instance;
        }

        public TeamCityInstance Instance { get; }
        public ITeamCityService Service => Instance.Service; 
    }

    // private class UserLocatorImpl(private val instance: TeamCityInstanceImpl): UserLocator
    // {
    //     private var id: UserId ? = null
    //     private var username: String ? = null

    //     override fun WithId(id: UserId): UserLocator
    // {
    //     this.id = id
    //         return this
    //     }
}