using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class User : Base<UserDto>, IUser
    {
        private User(UserDto dto, TeamCityServer instance)
            : base(dto, instance) { }

        public static async Task<IUser> Create(string idString, TeamCityServer instance)
        {
            var dto = await instance.Service.Users($"id:{idString}").ConfigureAwait(false);
            return new User(dto, instance);
        }

        public UserId Id => new UserId(IdString);
        public string Username => Dto.Username;
        public string Name => Dto.Name;
        public string Email => Dto.Email;
        public string GetHomeUrl()
            => Instance.GetUserUrlPage("admin/editUser.html", userId: Id);

        public override string ToString()
            => $"User(id={Id.stringId}, username={Username})";
    }
}