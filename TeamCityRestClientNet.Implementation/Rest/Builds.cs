using System.Collections.Generic;
using Newtonsoft.Json;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.RestApi
{
    public class BuildListDto : ListDto<BuildDto>
    {
        [JsonProperty(PropertyName ="build")]
        public override List<BuildDto> Items { get; set; } = new List<BuildDto>();
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

    public class BuildCanceledDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
    }

    public class TagsDto
    {
        public List<TagDto> Tag { get; set; } = new List<TagDto>();
    }

    public class TagDto
    {
        public string Name { get; set; }
    }

    public class BuildRunningInfoDto
    {
        public int PercentageComplete { get; set; } = 0;
        public long ElapsedSeconds { get; set; } = 0;
        public long EstimatedTotalSeconds { get; set; } = 0;
        public bool Outdated { get; set; } = false;
        public bool ProbablyHanging { get; set; } = false;
    }

    public class RevisionsDto
    {
        public List<RevisionDto> Revision { get; set; } = new List<RevisionDto>();
    }

    public class RevisionDto
    {
        public string Version { get; set; }
        public string VcsBranchName { get; set; }
        public VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    public class PinInfoDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
    }

    public class TriggeredDto
    {
        public UserDto User { get; set; }
        public BuildDto Build { get; set; }
    }

    public class BuildCommentDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
    }

    public class ArtifactFileListDto
    {
        public List<ArtifactFileDto> File { get; set; } = new List<ArtifactFileDto>();
    }

    public class ArtifactFileDto
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public long? Size { get; set; }
        public string ModificationTime { get; set; }
        public static readonly string FIELDS
            = $"{nameof(FullName)},{nameof(Name)},${nameof(Size)},${nameof(ModificationTime)}";
    }
}