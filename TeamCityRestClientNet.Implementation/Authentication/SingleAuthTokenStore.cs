using System.Threading.Tasks;

namespace TeamCityRestClientNet.Authentication
{
    public class SingleAuthTokenStore : IAuthTokenStore
    {
        private readonly string _token;

        public SingleAuthTokenStore(string token) => _token = token;

        public async Task<string> GetToken() => await Task.FromResult(_token);
    }
}