using System;
using System.Collections.Generic;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class DataBuilder
    {
        #region Builds

        private readonly BuildDto BuildOK = new BuildDto
        {
            Id = "101",
            BuildTypeId = "TeamCityRestClientNet_RestClient",
            Number = "1",
            Status = BuildStatus.SUCCESS,
            State = "finished",
            BranchName = "refs/heads/master",
            DefaultBranch = true,
            StatusText = "Tests passed: 56",
            BuildType = new BuildTypeDto
            {
                Id = "TeamCityRestClientNet_RestClient",
                Name = "Rest Client",
                ProjectId = "TeamCityRestClientNet"                
            },
            QueuedDate = "20210103T093500+0000",
            StartDate = "20210103T094000+0000",
            FinishDate = "20210103T095000+0000",
            Triggered = new TriggeredDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                }
            },
            Comment = new BuildCommentDto 
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                },
                Timestamp = "20210103T093500+0000",
                Text = "Building"
            },
            Revisions = new RevisionsDto
            {
                Revision = new List<RevisionDto> 
                {
                    new RevisionDto 
                    {
                        Version = "54f7ef0ebcc5be2f59a66543034740c9b7383cec",
                        VcsBranchName = "refs/heads/master",
                        VcsRootInstance = new VcsRootInstanceDto
                        {
                            VcsRootId = "TeamCityRestClientNet_Bitbucket",
                            Id = "3"
                        }
                    }
                }
            },
            Agent = new BuildAgentDto
            {
                Id = "1",
                Name = "ip_172.17.0.3"
            },
            Properties = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("param1", "value1")
                }
            }
        };

        private readonly BuildDto BuildFailed = new BuildDto
        {
            Id = "102",
            BuildTypeId = "TeamCityRestClientNet_RestClient",
            Number = "2",
            Status = BuildStatus.FAILURE,
            State = "finished",
            StatusText = "/usr/share/dotnet/sdk/3.1.403/NuGet.targets(128,5): error : Unable to load the service index for source https://api.nuget.org/v3/index.json. /usr/share/dotnet/sdk/3.1.403/NuGet.targets(128,5): error : Resource temporarily unavailable (new);",
            BuildType = new BuildTypeDto
            {
                Id = "TeamCityRestClientNet_RestClient",
                Name = "Rest Client",
                ProjectId = "TeamCityRestClientNet"
            },
            QueuedDate = "20210104T093500+0000",
            StartDate = "20210104T094000+0000",
            FinishDate = "20210104T095000+0000",
            Triggered = new TriggeredDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                }
            },
            Revisions = new RevisionsDto
            {
                Revision = new List<RevisionDto>
                {
                    new RevisionDto
                    {
                        Version = "2d5fc7874d73d0e4b7ae555bd00be14f79b76154",
                        VcsBranchName = "refs/heads/master",
                        VcsRootInstance = new VcsRootInstanceDto
                        {
                            VcsRootId = "TeamCityRestClientNet_Bitbucket",
                            Id = "3"
                        }
                    }
                }
            },
            Agent = new BuildAgentDto
            {
                Id = "1",
                Name = "ip_172.17.0.3"
            }
        };

        private readonly BuildDto BuildQueued = new BuildDto
        {
            Id = "103",
            BuildTypeId = "TeamCityRestClientNet_RestClient",
            State = "queued",
            BranchName = "<default>",
            DefaultBranch = true,
            BuildType = new BuildTypeDto
            {
                Id = "TeamCityRestClientNet_RestClient",
                Name = "Rest Client",
                ProjectId = "TeamCityRestClientNet"
            },
            QueuedDate = "20210104T193500+0000",
            Triggered = new TriggeredDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                }
            },
            Properties = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("param1", "value1")
                }
            }
        };

        private readonly BuildDto BuildCancelled = new BuildDto
        {
            Id = "104",
            BuildTypeId = "TeamCityRestClientNet_RestClient",
            Number = "N/A",
            Status = BuildStatus.UNKNOWN,
            State = "finished",
            BranchName = "<default>",
            DefaultBranch = true,
            StatusText = "Canceled",
            BuildType = new BuildTypeDto
            {
                Id = "TeamCityRestClientNet_RestClient",
                Name = "Rest Client",
                ProjectId = "TeamCityRestClientNet"
            },
            CanceledInfo = new BuildCanceledDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                },
                Timestamp = "20210103T093500+0000",
                Text = "Cancelled-1414ceb1-f7ae-42e4-b7c1-d18159e99506"
            },
            QueuedDate = "20210103T093500+0000",
            StartDate = "20210103T094000+0000",
            FinishDate = "20210103T095000+0000",
            Triggered = new TriggeredDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                }
            }
        };

        private readonly BuildDto BuildRunning = new BuildDto
        {
            Id = "105",
            BuildTypeId = "TeamCityRestClientNet_RestClient",
            Number = "105",
            Status = BuildStatus.UNKNOWN,
            State = "running",
            BranchName = "<default>",
            DefaultBranch = true,
            StatusText = "Running",
            BuildType = new BuildTypeDto
            {
                Id = "TeamCityRestClientNet_RestClient",
                Name = "Rest Client",
                ProjectId = "TeamCityRestClientNet"
            },
            QueuedDate = "20210103T093500+0000",
            StartDate = "20210103T094000+0000",
            Triggered = new TriggeredDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                }
            }
        };

        #endregion
 
        #region BuildAgents

        public readonly BuildAgentDto AgentEnabled = new BuildAgentDto
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
            EnabledInfo = new EnabledInfoDto(),
            AuthorizedInfo = new AuthorizedInfoDto(),
            Pool = new BuildAgentPoolDto
            {
                Id = "0",
                Name = "Default"
            }
        };

        public readonly BuildAgentDto AgentDisabled = new BuildAgentDto
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
            AuthorizedInfo = new AuthorizedInfoDto(),
            Pool = new BuildAgentPoolDto
            {
                Id = "0",
                Name = "Default"
            }
        };

        #endregion

        #region BuildAgentPools

        public readonly BuildAgentPoolDto DefaultPool = new BuildAgentPoolDto
        {
            Id = "0",
            Name = "Default"
        };

        #endregion

        #region BuildTypes
        private readonly BuildTypeDto BuildTypeRestClient = new BuildTypeDto
        {
            Id = "TeamCityRestClientNet_RestClient",
            Name = "Rest Client",
            ProjectId = "TeamCityRestClientNet",
            Settings = new BuildTypeSettingsDto
            {
                Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "artifactRules", Value = "artifacts" },
                    new NameValuePropertyDto { Name = "buildNumberCounter", Value = "138" },
                    new NameValuePropertyDto { Name = "cleanBuild", Value = "true" },
                    new NameValuePropertyDto { Name = "publishArtifactCondition", Value = "SUCCESSFUL" }
                }
            }
        };

        private readonly BuildTypeDto BuildTypeTeamCityCli = new BuildTypeDto
        {
            Id = "TeamCityCliNet_Cli",
            Name = "CLI",
            ProjectId = "TeamCityCliNet",
            Settings = new BuildTypeSettingsDto
            {
                Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "buildNumberCounter", Value = "1" }
                }
            }
        };
        #endregion

        #region Changes

        private readonly ChangeDto Change1 = new ChangeDto
        {
            Id = "1",
            Username = "jodoe",
            Version = "a9f57192-48d1-4e7a-b3f5-ebead0c6f8d6",
            Comment = "Initial commit",
            Date = DateTime.UtcNow.AddDays(-7).ToString(Constants.TEAMCITY_DATETIME_FORMAT),
            User = new UserDto
            {
                Id = "1",
                Name = "John Doe",
                Username = "jodoe"
            },
            VcsRootInstance = new VcsRootInstanceDto
            {
                Id = "3",
                VcsRootId = "TeamCityRestClientNet_Bitbucket",
                Name = "Bitbucket"
            }
        };

        private readonly ChangeDto Change2 = new ChangeDto
        {
            Id = "2",
            Username = "jodoe",
            Version = "7d366624-138f-4ff6-9393-1e24556ffbf8",
            Comment = "Add TeamCity fake data.",
            Date = DateTime.UtcNow.AddDays(-6).ToString(Constants.TEAMCITY_DATETIME_FORMAT),
            User = new UserDto
            {
                Id = "1",
                Name = "John Doe",
                Username = "jodoe"
            },
            VcsRootInstance = new VcsRootInstanceDto
            {
                Id = "3",
                VcsRootId = "TeamCityRestClientNet_Bitbucket",
                Name = "Bitbucket"
            }
        };

        private readonly ChangeDto Change3 = new ChangeDto
        {
            Id = "3",
            Username = "jodoe",
            Version = "c5e84027-bd12-41cb-8bc9-e0609b6a6d66",
            Comment = "Add Changes unit tests",
            Date = DateTime.UtcNow.AddDays(-4).ToString(Constants.TEAMCITY_DATETIME_FORMAT),
            User = new UserDto
            {
                Id = "1",
                Name = "John Doe",
                Username = "jodoe"
            },
            VcsRootInstance = new VcsRootInstanceDto
            {
                Id = "3",
                VcsRootId = "TeamCityRestClientNet_Bitbucket",
                Name = "Bitbucket"
            }
        };

        #endregion

        #region Projects
        private readonly ProjectDto RootProject = new ProjectDto
        {
            Id = "_Root",
            Name = "<Root project>"
        };

        private readonly ProjectDto RestClientProject = new ProjectDto
        {
            Id = "TeamCityRestClientNet",
            ParentProjectId = "_Root",
            Name = "TeamCity Rest Client .NET",
            Parameters = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("configuration_parameter", "6692e7bf_c9a4_4941_9e89_5dde9417f05f"),
                }
            }
        };

        private readonly ProjectDto TeamCityCliProject = new ProjectDto
        {
            Id = "TeamCityCliNet",
            ParentProjectId = "_Root",
            Name = "TeamCity CLI .NET"
        };

        private readonly ProjectDto Project1 = new ProjectDto
        {
            Id = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4",
            ParentProjectId = "_Root",
            Name = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4"
        };

        private readonly ProjectDto Project2 = new ProjectDto
        {
            Id = "Project_1cd586a8_d65c_44b1_b60e_e63f8b471819",
            ParentProjectId = "_Root",
            Name = "Project_1cd586a8_d65c_44b1_b60e_e63f8b471819"
        };

        private readonly ProjectDto Project3 = new ProjectDto
        {
            Id = "Project_3a1ac261_96d4_45b0_ac3d_7245718a3928",
            ParentProjectId = "_Root",
            Name = "Project_3a1ac261_96d4_45b0_ac3d_7245718a3928"
        };
        #endregion

        #region Users
        private readonly UserDto UserJohnDoe = new UserDto
        {
            Id = "1",
            Name = "John Doe",
            Username = "jodoe",
            Email = "john.doe@mailinator.com"
        };

        private readonly UserDto UserJaneDoe = new UserDto
        {
            Id = "2",
            Name = "Jane Doe",
            Username = "jadoe",
            Email = "jane.doe@mailinator.com"
        };

        private readonly UserDto UserDunkinDonuts = new UserDto
        {
            Id = "3",
            Name = "Dunkin' Donuts",
            Username = "dunkin",
            Email = "dunkin@mailinator.com"
        };

        private readonly UserDto UserMacCheese = new UserDto
        {
            Id = "4",
            Name = "Mac Cheese",
            Username = "maccheese",
            Email = "maccheese@mailinator.com"
        };
        #endregion

        #region VcsRoots
        private readonly VcsRootDto VcsRestClientGit = new VcsRootDto
        {
            Id = "TeamCityRestClientNet_Bitbucket",
            Name = "Bitbucket",
            Properties = new NameValuePropertiesDto
            {
                Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "agentCleanFilesPolicy", Value = "ALL_UNTRACKED" },
                    new NameValuePropertyDto { Name = "agentCleanPolicy", Value = "ON_BRANCH_CHANGE" },
                    new NameValuePropertyDto { Name = "authMethod", Value = "PASSWORD" },
                    new NameValuePropertyDto { Name = "branch", Value = "refs/heads/master" },
                    new NameValuePropertyDto { Name = "ignoreKnownHosts", Value = "true" },
                    new NameValuePropertyDto { Name = "secure:password" },
                    new NameValuePropertyDto { Name = "submoduleCheckout", Value = "CHECKOUT" },
                    new NameValuePropertyDto { Name = "teamcity:branchSpec", Value="+:*" },
                    new NameValuePropertyDto { Name = "url", Value = "https://noexist@bitbucket.org/joedoe/teamcityrestclientnet.git" },
                    new NameValuePropertyDto { Name = "useAlternates", Value = "true" },
                    new NameValuePropertyDto { Name = "usernameStyle", Value = "USERID" }
                }
            }
        };

        private readonly VcsRootDto Vcs1 = new VcsRootDto
        {
            Id = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269",
            Name = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269"
        };

        private readonly VcsRootDto Vcs2 = new VcsRootDto
        {
            Id = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6",
            Name = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6"
        };

        private readonly VcsRootDto Vcs3 = new VcsRootDto
        {
            Id = "Vcs_ExtraOne",
            Name = "Vcs_ExtraOne"
        };

        private readonly VcsRootDto Vcs4 = new VcsRootDto
        {
            Id = "Vcs_AnotherOne",
            Name = "Vcs_AnotherOne"
        };

        #endregion

        #region VcsRootInstances
       
        private readonly VcsRootInstanceDto VcsInstance1 = new VcsRootInstanceDto
        {
            Id = "3",
            VcsRootId = "TeamCityRestClientNet_Bitbucket",
            Name = "Bitbucket"
        };

        #endregion
        
        public DataBuilder()
        {
            var builds = new BuildRepository();
            Builds = builds;
            BuildAgents = new BuildAgentRepository();
            BuildAgentPools = new BuildAgentPoolRepository();
            BuildTypes = new BuildTypeRepository(builds);
            Changes = new ChangeRepository();
            Projects = new ProjectRepository();
            Users = new UserRepository();
            VcsRoots = new VcsRootRepository();
            VcsRootInstances = new VcsRootInstanceRepository();
        }

        public void Load()
        {
            var authComment = AgentEnabled.AuthorizedInfo.Comment;
            authComment.Timestamp = DateTime.UtcNow.AddDays(-14).ToString(Constants.TEAMCITY_DATETIME_FORMAT);
            authComment.Text = "Authorized";
            authComment.User = UserJohnDoe;
            var enabComment = AgentEnabled.EnabledInfo.Comment;
            enabComment.Timestamp = DateTime.UtcNow.AddDays(-13).ToString(Constants.TEAMCITY_DATETIME_FORMAT);
            enabComment.Text = "Enabled";
            enabComment.User = UserJohnDoe;
            BuildAgents.Add(AgentEnabled);

            authComment = AgentDisabled.AuthorizedInfo.Comment;
            authComment.Timestamp = DateTime.UtcNow.AddDays(-24).ToString(Constants.TEAMCITY_DATETIME_FORMAT);
            authComment.Text = "Authorized disabled agent";
            authComment.User = UserJaneDoe;
            BuildAgents.Add(AgentDisabled);

            DefaultPool.Agents.Items.Add(AgentEnabled);
            DefaultPool.Agents.Items.Add(AgentDisabled);
            DefaultPool.Projects.Items.Add(RestClientProject);
            DefaultPool.Projects.Items.Add(TeamCityCliProject);
            DefaultPool.Projects.Items.Add(Project1);
            DefaultPool.Projects.Items.Add(Project2);
            DefaultPool.Projects.Items.Add(Project3);
            BuildAgentPools.Add(DefaultPool);

            Builds.Add(BuildOK);
            Builds.Add(BuildFailed);
            Builds.Add(BuildQueued);
            Builds.Add(BuildCancelled);
            Builds.Add(BuildRunning);
            
            BuildTypes.Add(BuildTypeRestClient);
            BuildTypes.Add(BuildTypeTeamCityCli);

            Changes.Add(Change1);
            Changes.Add(Change2);
            Changes.Add(Change3);

            RootProject.Projects.Items.Add(RestClientProject);
            RootProject.Projects.Items.Add(TeamCityCliProject);
            RootProject.Projects.Items.Add(Project1);
            RootProject.Projects.Items.Add(Project2);
            RootProject.Projects.Items.Add(Project3);
            Projects.Add(RootProject);
            RestClientProject.BuildTypes.Items.Add(BuildTypeRestClient);
            Projects.Add(RestClientProject);
            TeamCityCliProject.BuildTypes.Items.Add(BuildTypeTeamCityCli);
            Projects.Add(TeamCityCliProject);
            Projects.Add(Project1);
            Projects.Add(Project2);
            Projects.Add(Project3);

            Users.Add(UserJohnDoe);
            Users.Add(UserJaneDoe);
            Users.Add(UserDunkinDonuts);
            Users.Add(UserMacCheese);

            VcsRoots.Add(VcsRestClientGit);
            VcsRoots.Add(Vcs1);
            VcsRoots.Add(Vcs2);
            VcsRoots.Add(Vcs3);
            VcsRoots.Add(Vcs4);

            VcsRootInstances.Add(VcsInstance1);
        }

        public BuildRepository Builds { get; private set; }
        public BuildAgentRepository BuildAgents { get; private set; }
        public BuildAgentPoolRepository BuildAgentPools { get; private set; }
        public BuildTypeRepository BuildTypes { get; private set; }
        public ProjectRepository Projects { get; private set; }
        public ChangeRepository Changes { get; private set; }
        public UserRepository Users { get; private set; }
        public VcsRootRepository VcsRoots { get; private set; }
        public VcsRootInstanceRepository VcsRootInstances { get; private set; }
    }
}