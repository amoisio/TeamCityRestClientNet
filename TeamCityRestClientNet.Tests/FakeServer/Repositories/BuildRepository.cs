using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildRepository : BaseRepository<BuildDto, BuildListDto>
    {
        public void CancelBuild(string id)
        {
            var build = ById(id);
            build.Status = BuildStatus.UNKNOWN;
            build.StatusText = "Canceled";
            build.State = "finished";
            build.CanceledInfo = new BuildCanceledDto
            {
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                },
                Timestamp = "20210104T210609+0000",
                Text = "Cancelled-1414ceb1-f7ae-42e4-b7c1-d18159e99506"
            };
            build.StartDate = "20210104T210609+0000";
            build.FinishDate = "20210104T210609+0000";
        }

        public void AddTag(string id, string tag)
        {
            var build = ById(id);
            if (build.Tags == null)
                build.Tags = new TagsDto();

            build.Tags.Tag.Add(new TagDto
            {
                Name = tag
            });
        }

        public ParametersDto ResultingProperties(string id)
        {
            var build = ById(id);
            return build.Properties;
        }

        public ArtifactFileListDto Artifacts(string id)
        {
            return new ArtifactFileListDto
            {
                File = new List<ArtifactFileDto> 
                {
                    new ArtifactFileDto
                    {
                        Name = $"Artifact name {id}",
                        FullName = "Artifact full name",
                        Size = 10000,
                        ModificationTime = "20210104T210609+0000"
                    },
                    new ArtifactFileDto
                    {
                        Name = "Nuget",
                        FullName = "Full nuget name",
                        Size = 15000,
                        ModificationTime = "20210104T210609+0000"
                    }
                }
            };
        }

        public Stream Content(string id)
        {
            string content = $"This is a build artifact for {id}";
            var bytes = Encoding.UTF8.GetBytes(content);
            return new MemoryStream(bytes);
        }

        public void Unpin(string id)
        {
            var build = ById(id);
            build.PinInfo = null;
        }

        #pragma warning disable IDE0060
        public void Pin(string id, string comment)
        {
            var build = ById(id);
            build.PinInfo = new PinInfoDto
            {
                Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                },
                
            };
        }

        public void ReplaceTags(string id, TagsDto tagsDto)
        {
            var build = ById(id);
            build.Tags = tagsDto;
        }

        public void SetComment(string id, string comment)
        {
            var build = ById(id);
            build.Comment = new BuildCommentDto
            {
                Text = comment,
                Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                User = new UserDto
                {
                    Id = "1",
                    Username = "jodoe",
                    Name = "John Doe"
                }
            };
        }

        public BuildDto CreateOKBuild(BuildTypeDto buildType, UserDto user, BuildAgentDto agent, VcsRootInstanceDto instance) => new BuildDto
        {
            Id = "101",
            BuildTypeId = buildType.Id,
            Number = "1",
            Status = BuildStatus.SUCCESS,
            State = "finished",
            BranchName = "refs/heads/master",
            DefaultBranch = true,
            StatusText = "Tests passed: 56",
            BuildType = buildType,
            QueuedDate = "20210103T093500+0000",
            StartDate = "20210103T094000+0000",
            FinishDate = "20210103T095000+0000",
            Triggered = new TriggeredDto
            {
                User = user
            },
            Comment = new BuildCommentDto
            {
                User = user,
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
                        VcsRootInstance = instance
                    }
                }
            },
            Agent = agent,
            Properties = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("param1", "value1")
                }
            },
            Tags = new TagsDto
            {
                Tag = new List<TagDto>
                {
                    new TagDto
                    {
                        Name = "Release #1"
                    }
                }
            }
        };

        public BuildDto CreateFailedBuild(BuildTypeDto buildType, UserDto user, BuildAgentDto agent, VcsRootInstanceDto instance) => new BuildDto
        {
            Id = "102",
            BuildTypeId = buildType.Id,
            Number = "2",
            Status = BuildStatus.FAILURE,
            State = "finished",
            StatusText = "/usr/share/dotnet/sdk/3.1.403/NuGet.targets(128,5): error : Unable to load the service index for source https://api.nuget.org/v3/index.json. /usr/share/dotnet/sdk/3.1.403/NuGet.targets(128,5): error : Resource temporarily unavailable (new);",
            BuildType = buildType,
            QueuedDate = "20210104T093500+0000",
            StartDate = "20210104T094000+0000",
            FinishDate = "20210104T095000+0000",
            Triggered = new TriggeredDto
            {
                User = user
            },
            Revisions = new RevisionsDto
            {
                Revision = new List<RevisionDto>
                {
                    new RevisionDto
                    {
                        Version = "2d5fc7874d73d0e4b7ae555bd00be14f79b76154",
                        VcsBranchName = "refs/heads/master",
                        VcsRootInstance = instance
                    }
                }
            },
            Agent = agent
        };

        public BuildDto CreateQueuedBuild(BuildTypeDto buildType, UserDto user) => new BuildDto
        {
            Id = "103",
            BuildTypeId = buildType.Id,
            State = "queued",
            BranchName = "<default>",
            DefaultBranch = true,
            BuildType = buildType,
            QueuedDate = "20210104T193500+0000",
            Triggered = new TriggeredDto
            {
                User = user
            },
            Properties = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("param1", "value1")
                }
            }
        };

        public BuildDto CreateCancelledBuild(BuildTypeDto buildType, UserDto user) => new BuildDto
        {
            Id = "104",
            BuildTypeId = buildType.Id,
            Number = "N/A",
            Status = BuildStatus.UNKNOWN,
            State = "finished",
            BranchName = "<default>",
            DefaultBranch = true,
            StatusText = "Canceled",
            BuildType = buildType,
            CanceledInfo = new BuildCanceledDto
            {
                User = user,
                Timestamp = "20210103T093500+0000",
                Text = "Cancelled-1414ceb1-f7ae-42e4-b7c1-d18159e99506"
            },
            QueuedDate = "20210103T093500+0000",
            StartDate = "20210103T094000+0000",
            FinishDate = "20210103T095000+0000",
            Triggered = new TriggeredDto
            {
                User = user
            }
        };

        public BuildDto CreateRunningBuild(BuildTypeDto buildType, UserDto user) => new BuildDto
        {
            Id = "105",
            BuildTypeId = buildType.Id,
            Number = "105",
            Status = BuildStatus.UNKNOWN,
            State = "running",
            BranchName = "<default>",
            DefaultBranch = true,
            StatusText = "Running",
            BuildType = buildType,
            QueuedDate = "20210103T093500+0000",
            StartDate = "20210103T094000+0000",
            Triggered = new TriggeredDto
            {
                User = user
            }
        };
    }
}