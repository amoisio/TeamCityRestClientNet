using System.Collections.Generic;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildListDto
    {
        public string NextHref { get; set; }
        public List<BuildDto> Build { get; set; } = new List<BuildDto>();
    }

    public class BuildDto : IdDto
    {
        public string BuildTypeId { get; set; }
        public BuildCanceledDto CanceledInfo { get; set; }
        public string Number { get; set; }
        public BuildStatus? Status { get; set; }
        public string State { get; set; }
        public bool? Personal { get; set; }
        public string BranchName { get; set; }
        public bool? DefaultBranch { get; set; }
        public bool? Composite { get; set; }
        public string StatusText { get; set; }
        public string QueuedDate { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public TagsDto Tags { get; set; }
        public BuildRunningInfoDto RunningInfo { get; set; }
        public RevisionsDto Revisions { get; set; }
        public PinInfoDto PinInfo { get; set; }
        public TriggeredDto Triggered { get; set; }
        public BuildCommentDto Comment { get; set; }
        public BuildAgentDto Agent { get; set; }
        public ParametersDto Properties { get; set; } = new ParametersDto();
        public BuildTypeDto BuildType { get; set; } = new BuildTypeDto();
        public BuildListDto SnapshotDependencies { get; set; }
    }
}