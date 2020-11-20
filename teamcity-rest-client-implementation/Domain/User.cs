using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class User : Base<UserDto>, IUser
    {
        internal User(UserDto dto, bool isFullBuildDto, TeamCityInstance instance)
            : base(dto, isFullBuildDto, instance)
        {
            
        }

        public UserId Id => new UserId(IdString);
        public string Username => Dto.Username;
        public string Name => Dto.Name;
        public string Email => Dto.Email;
        public string GetHomeUrl()
            => Instance.GetUserUrlPage("admin/editUser.html", userId: Id);

        public override string ToString()
            => $"User(id={Id.stringId}, username={Username})";

        protected override async Task<UserDto> FetchFullDto()
        {
            return await Service.Users($"id:{Id.stringId}");
        }
    }
}