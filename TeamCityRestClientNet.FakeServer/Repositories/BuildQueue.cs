using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildQueue : List<BuildDto>
    {
        public TriggeredBuildDto TriggerBuild(BuildRepository builds, UserDto user, TriggerBuildRequestDto request)
        {
            int newId = builds.All().Items.Max(i => Int32.Parse(i.Id)) + 1;
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
                    User = user
                },
                Comment = new BuildCommentDto
                {
                    User = user,
                    Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    Text = request.Comment?.Text
                },
                Properties = request.Properties
            };

            this.Add(newBuild);
            builds.Add(newBuild);

            return new TriggeredBuildDto
            {
                Id = newId,
                BuildTypeId = request.BuildType.Id
            };
        }

        public void CancelBuild(string id, UserDto user, BuildCancelRequestDto request)
        {
            var queuedBuild = this.FirstOrDefault(build => build.Id == id);
            if (queuedBuild != null)
            {
                queuedBuild.State = "finished";
                queuedBuild.Status = BuildStatus.UNKNOWN;
                queuedBuild.StatusText  = "Canceled";
                queuedBuild.CanceledInfo = new BuildCanceledDto
                {
                    User = user,
                    Timestamp = DateTime.UtcNow.ToString(Constants.TEAMCITY_DATETIME_FORMAT),
                    Text = request.Comment
                };
            }
            this.Remove(queuedBuild);
        }
    }
}