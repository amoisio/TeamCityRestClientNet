using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public interface ITokenStore
    {
        Task<string> GetToken();
    }

    public interface IBearerTokenStore : ITokenStore { }

    public class SingleBearerTokenStore : IBearerTokenStore
    {
        private readonly string _token;

        public SingleBearerTokenStore(string token) => _token = token;

        public async Task<string> GetToken() => await Task.FromResult(_token);
    }

    public interface ICSRFTokenStore : ITokenStore { }

    public class TeamCityCSRFTokenStore : ICSRFTokenStore
    {
        private readonly ITeamCityAuthService _authService;

        public TeamCityCSRFTokenStore(ITeamCityAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> GetToken()
        {
            return await _authService.CSRFToken().ConfigureAwait(false);
        }
    }
}