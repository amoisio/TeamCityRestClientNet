using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Implementations
{
    class Triggered : ITriggeredInfo
    {
        private readonly TriggeredDto _dto;
        private readonly TeamCityInstance _instance;

        public Triggered(TriggeredDto dto, TeamCityInstance instance)
        {
            this._dto = dto;
            this._instance = instance;
        }

        public IUser User 
            => this._dto.User.Let(dto => new User(dto, false, this._instance));

        public IBuild Build
            => this._dto.Build.Let(dto => new Build(dto, false, this._instance));
    }
}