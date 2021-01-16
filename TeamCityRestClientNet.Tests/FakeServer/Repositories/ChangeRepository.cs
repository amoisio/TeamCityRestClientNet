using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class ChangeRepository : BaseRepository<ChangeDto, ChangeListDto>
    {
        public ChangeDto CreateChange(string id, string comment, DateTime date, UserDto user, VcsRootInstanceDto instance)
        {
            var change = new ChangeDto
            {
                Id = id,
                Username = user.Username,
                Version = Guid.NewGuid().ToString(),
                Comment = comment,
                Date = date.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                User = user,
                VcsRootInstance = instance
            };
            return change;
        }
    }
}