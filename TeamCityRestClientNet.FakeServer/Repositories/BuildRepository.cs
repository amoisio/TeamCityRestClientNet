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

        internal ParametersDto ResultingProperties(string id)
        {
            var build = ById(id);
            return build.Properties;
        }

        internal ArtifactFileListDto Artifacts(string id)
        {
            return new ArtifactFileListDto
            {
                File = new List<ArtifactFileDto> 
                {
                    new ArtifactFileDto
                    {
                        Name = "Artifact name",
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

        internal Stream Content(string id)
        {
            string content = $"This is a build artifact for {id}";
            var bytes = Encoding.UTF8.GetBytes(content);
            return new MemoryStream(bytes);
        }

        internal void Unpin(string id)
        {
            var build = ById(id);
            build.PinInfo = null;
        }

        internal void Pin(string id, string comment)
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
                }
            };
        }

        internal void ReplaceTags(string id, TagsDto tagsDto)
        {
            var build = ById(id);
            build.Tags = tagsDto;
        }

        internal void SetComment(string id, string comment)
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
    }
}