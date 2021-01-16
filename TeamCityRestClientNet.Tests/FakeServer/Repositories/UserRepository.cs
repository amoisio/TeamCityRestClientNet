using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class UserRepository : BaseRepository<UserDto, UserListDto>
    {
        public UserDto ByUsername(string username) => _itemsById.Values.SingleOrDefault(u => u.Username == username);

        public UserDto CreateJohnDoe() => new UserDto
        {
            Id = "1",
            Name = "John Doe",
            Username = "jodoe",
            Email = "john.doe@mailinator.com"
        };

        public UserDto CreateJaneDoe() => new UserDto
        {
            Id = "2",
            Name = "Jane Doe",
            Username = "jadoe",
            Email = "jane.doe@mailinator.com"
        };

        public UserDto CreateDunkinDonuts() => new UserDto
        {
            Id = "3",
            Name = "Dunkin' Donuts",
            Username = "dunkin",
            Email = "dunkin@mailinator.com"
        };

        public UserDto CreateMacCheese() => new UserDto
        {
            Id = "4",
            Name = "Mac Cheese",
            Username = "maccheese",
            Email = "maccheese@mailinator.com"
        };
    }
}