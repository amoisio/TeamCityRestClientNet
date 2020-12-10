using System.IO;
using System.Threading.Tasks;
using Refit;

namespace TeamCityRestClientNet.Authentication
{
    public interface ITeamCityAuthService
    {
        //         To obtain a security token, send the GET https://your-server/authenticationTest.html?csrf request.
        // To pass the token, use the X-TC-CSRF-Token HTTP request header or the tc-csrf-token HTTP parameter.
        [Headers("Accept: application/json")]
        [Get("authenticationTest.html?csrf")]
        Task<string> CSRFToken();
    }
}