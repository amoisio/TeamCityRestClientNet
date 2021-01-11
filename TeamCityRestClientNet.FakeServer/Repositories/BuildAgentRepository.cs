using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildAgentRepository : BaseRepository<BuildAgentDto, BuildAgentListDto>
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

        public BuildAgentDto CreateEnabledAgent(UserDto enabler, UserDto authorizer, BuildAgentPoolDto agentPool)
        {
            var agent = new BuildAgentDto
            {
                Id = "1",
                Name = "ip_172.17.0.3",
                Connected = true,
                Enabled = true,
                Authorized = true,
                Uptodate = true,
                Ip = "172.17.0.3",
                Properties = new ParametersDto
                {
                    Property = new List<ParameterDto>
                {
                    new ParameterDto("env._", "/opt/buildagent/bin/agent.sh"),
                    new ParameterDto("env.ASPNETCORE_URLS", "http://+:80"),
                    new ParameterDto("env.CONFIG_FILE", "/data/teamcity_agent/conf/buildAgent.properties"),
                    new ParameterDto("env.DEBIAN_FRONTEND", "noninteractive"),
                    new ParameterDto("env.DOTNET_CLI_TELEMETRY_OPTOUT", "true"),
                    new ParameterDto("env.DOTNET_RUNNING_IN_CONTAINER", "true"),
                    new ParameterDto("env.DOTNET_SDK_VERSION", "3.1.403"),
                    new ParameterDto("env.DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true"),
                    new ParameterDto("env.DOTNET_USE_POLLING_FILE_WATCHER", "true"),
                    new ParameterDto("env.GIT_SSH_VARIANT", "ssh")
                }
                },
                EnabledInfo = new EnabledInfoDto
                {
                    Comment = new EnabledInfoCommentDto
                    {
                        Timestamp = DateTime.UtcNow.AddDays(-13).ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                        Text = "Enabled",
                        User = enabler
                    }
                },
                AuthorizedInfo = new AuthorizedInfoDto
                {
                    Comment = new AuthorizedInfoCommentDto
                    {
                        Timestamp = DateTime.UtcNow.AddDays(-14).ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                        Text = "Authorized",
                        User = authorizer
                    }
                },
                Pool = new BuildAgentPoolDto
                {
                    Name = agentPool.Name,
                    Projects = new ProjectListDto
                    {
                        Items = agentPool.Projects.Items.Select(i => new ProjectDto
                        {
                            Id = i.Id
                        }).ToList()
                    },
                    Agents = new BuildAgentListDto
                    {
                        Items = agentPool.Projects.Items.Select(i => new BuildAgentDto
                        {
                            Id = i.Id
                        }).ToList()
                    }
                }
            };

            agentPool.Agents.Items.Add(agent);
            return agent;
        }

        public BuildAgentDto CreateDisabledAgent(UserDto authorizer, BuildAgentPoolDto agentPool) => new BuildAgentDto
        {
            Id = "2",
            Name = "Disabled build agent",
            Connected = true,
            Enabled = false,
            Authorized = true,
            Uptodate = true,
            Ip = "172.17.0.4",
            Properties = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("env.ASPNETCORE_URLS", "http://+:80"),
                }
            },
            EnabledInfo = null,
            AuthorizedInfo = new AuthorizedInfoDto
            {
                Comment = new AuthorizedInfoCommentDto
                {
                    Timestamp = DateTime.UtcNow.AddDays(-24).ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    Text = "Authorized disabled agent",
                    User = authorizer
                }
            },
            Pool = agentPool
        };
    }
}