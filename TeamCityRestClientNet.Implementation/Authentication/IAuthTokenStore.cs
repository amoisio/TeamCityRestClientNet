using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public interface IAuthTokenStore
    {
        Task<string> GetToken();
    }
}