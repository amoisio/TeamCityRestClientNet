using System.Threading.Tasks;
using Refit;

namespace TeamCityRestClientNet.Authentication
{
    public interface ITeamCityAuthService
    {
        [Headers("Accept: */*")]
        [Get("/authenticationTest.html?csrf=true")]
        Task<string> CSRFToken();
    }
}