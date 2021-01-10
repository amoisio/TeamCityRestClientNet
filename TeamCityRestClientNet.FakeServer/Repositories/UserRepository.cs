using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class UserRepository : BaseRepository<UserDto, UserListDto>
    {
        public UserRepository()
        {
            _itemsById.Add(UserJohnDoe.Id, UserJohnDoe);
            _itemsById.Add(UserJaneDoe.Id, UserJaneDoe);
            _itemsById.Add(UserDunkinDonuts.Id, UserDunkinDonuts);
            _itemsById.Add(UserMacCheese.Id, UserMacCheese);
        }

        public UserDto ByUsername(string username) => _itemsById.Values.SingleOrDefault(u => u.Username == username);

        public static UserDto UserJohnDoe = new UserDto
        {
            Id = "1",
            Name = "John Doe",
            Username = "jodoe",
            Email = "john.doe@mailinator.com"
        };

        public static UserDto UserJaneDoe = new UserDto
        {
            Id = "2",
            Name = "Jane Doe",
            Username = "jadoe",
            Email = "jane.doe@mailinator.com"
        };

        public static UserDto UserDunkinDonuts = new UserDto
        {
            Id = "3",
            Name = "Dunkin' Donuts",
            Username = "dunkin",
            Email = "dunkin@mailinator.com"
        };

        public static UserDto UserMacCheese = new UserDto
        {
            Id = "4",
            Name = "Mac Cheese",
            Username = "maccheese",
            Email = "maccheese@mailinator.com"
        };
    }
}