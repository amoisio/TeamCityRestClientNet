using System;
using System.Linq;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildAgentRepository : BaseRepository<BuildAgentDto, BuildAgentsDto>
    {
        public BuildAgentDto Enable(string id)
        {
            var agent = ById(id);
            agent.Enabled = true;
            agent.EnabledInfo = new EnabledInfoDto
            {
                Comment = new EnabledInfoCommentDto
                {
                    Text = null,
                    Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    User = new UserDto
                    {
                        Id = "1"
                    }
                }
            };
            return agent;
        }

        public BuildAgentDto Disable(string id)
        {
            var agent = ById(id);
            agent.Enabled = false;
            agent.EnabledInfo = new EnabledInfoDto
            {
                Comment = new EnabledInfoCommentDto
                {
                    Text = null,
                    Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    User = new UserDto
                    {
                        Id = "1"
                    }
                }
            };
            return agent;
        }
    }
}