using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    abstract class Locator 
    {
        protected readonly TeamCityInstance _instance;

        public Locator(TeamCityInstance instance)
        {
            this._instance = instance;
        }
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