using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class UserRepository : BaseRepository<UserDto, UserListDto>
    {
        public UserDto ByUsername(string username) => _itemsById.Values.SingleOrDefault(u => u.Username == username);
    }
}