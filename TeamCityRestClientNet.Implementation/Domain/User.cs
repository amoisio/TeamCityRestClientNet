using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Domain
{
    class User : Base<UserDto>, IUser
    {
        private User(UserDto dto, TeamCityServer instance)
            : base(dto, instance) { }

        public static async Task<IUser> Create(string idString, TeamCityServer instance)
            => await Create(new UserDto { Id = idString }, false, instance).ConfigureAwait(false);

        public static async Task<IUser> Create(UserDto dto, bool isFullDto, TeamCityServer instance)
        {
            var fullDto = isFullDto
                ? dto
                : await instance.Service.Users($"id:{dto.Id}").ConfigureAwait(false);
            return new User(fullDto, instance);
        }

        public string Username => Dto.Username;
        public string Name => Dto.Name;
        public string Email => Dto.Email;
        public string GetHomeUrl()
            => Instance.GetUserUrlPage("admin/editUser.html", userId: Id);

        public override string ToString()
            => $"User(id={Id}, username={Username})";
    }
}