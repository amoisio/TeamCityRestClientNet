using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildQueue : List<BuildDto>
    {
        private readonly BuildRepository _builds;
        public BuildQueue(BuildRepository builds)
        {
            this._builds = builds;
            this.AddRange(_builds.All().Items.Where(build => build.State == "queued"));
        }

        internal TriggeredBuildDto TriggerBuild(TriggerBuildRequestDto request)
        {
            var newId = _builds.All().Items.Max(item => Int32.Parse(item.Id)) + 1;
            var newBuild = new BuildDto
            {
                Id = newId.ToString(),
                BuildTypeId = request.BuildType.Id,
                Status = BuildStatus.UNKNOWN,
                State = "queued",
                BranchName = request.BranchName,
                DefaultBranch = true,
                BuildType = request.BuildType,
                QueuedDate = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
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
                    Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    Text = request.Comment?.Text
                },
                Properties = request.Properties
            };

            this.Add(newBuild);
            _builds.Add(newBuild);

            return new TriggeredBuildDto
            {
                Id = newId,
                BuildTypeId = request.BuildType.Id
            };
        }

        internal void CancelBuild(string id, BuildCancelRequestDto request)
        {
            var queuedBuild = this.FirstOrDefault(build => build.Id == id);
            if (queuedBuild != null)
            {
                queuedBuild.State = "finished";
                queuedBuild.Status = BuildStatus.UNKNOWN;
                queuedBuild.StatusText  = "Canceled";
                queuedBuild.CanceledInfo = new BuildCanceledDto
                {
                    User = new UserDto
                    {
                        Id = "1",
                        Username = "jodoe",
                        Name = "John Doe"
                    },
                    Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    Text = request.Comment
                };
            }
            this.Remove(queuedBuild);
        }
    }
}