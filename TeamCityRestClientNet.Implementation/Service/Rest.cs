using System.Collections.Generic;
using Newtonsoft.Json;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Service
{
    class ProjectsDto
    {
        public List<ProjectDto> Project { get; set; } = new List<ProjectDto>();
    }

    class BuildAgentsDto
    {
        public List<BuildAgentDto> Agent { get; set; } = new List<BuildAgentDto>();
    }

    class BuildAgentPoolsDto
    {
        public List<BuildAgentPoolDto> AgentPool { get; set; } = new List<BuildAgentPoolDto>();
    }

    class ArtifactFileListDto
    {
        public List<ArtifactFileDto> File { get; set; } = new List<ArtifactFileDto>();
    }

    class ArtifactFileDto
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public long? Size { get; set; }
        public string ModificationTime { get; set; }
        public static readonly string FIELDS 
            = $"{nameof(FullName)},{nameof(Name)},${nameof(Size)},${nameof(ModificationTime)}";
    }

    class IdDto
    {
        public string Id { get; set; }
    }

    class VcsRootListDto
    {
        public string Href { get; set; }
        public string NextHref { get; set; }
        
        [JsonProperty(PropertyName="vcs-root")]
        public List<VcsRootDto> VcsRoot { get; set; }
    }

    class VcsRootDto : IdDto
    {
        public string Name { get; set; }
        public NameValuePropertiesDto Properties { get; set; }
    }

    class VcsRootInstanceDto
    {
        public string VcsRootId { get; set; }
        public string Name { get; set; }
    }

    class BuildListDto
    {
        public string NextHref { get; set; }
        public List<BuildDto> Build { get; set; } = new List<BuildDto>();
    }

    class UserListDto
    {
        public List<UserDto> User { get; set; } = new List<UserDto>();
    }

    class BuildDto : IdDto
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

    class BuildRunningInfoDto
    {
        public int PercentageComplete { get; set; } = 0;
        public long ElapsedSeconds { get; set; } = 0;
        public long EstimatedTotalSeconds { get; set; } = 0;
        public bool Outdated { get; set; } = false;
        public bool ProbablyHanging { get; set; } = false;
    }

    class BuildTypeDto : IdDto
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public bool? Paused { get; set; }
        public BuildTypeSettingsDto Settings { get; set; }
    }

    class BuildTypeSettingsDto
    {
        public List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    class BuildProblemDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Identity { get; set; }
    }

    class BuildProblemOccurrencesDto
    {
        public string NextHref { get; set; }
        public List<BuildProblemOccurrenceDto> ProblemOccurrence { get; set; } = new List<BuildProblemOccurrenceDto>();
    }

    class BuildProblemOccurrenceDto
    {
        public string Details { get; set; }
        public string AdditionalData { get; set; }
        public BuildProblemDto Problem { get; set; }
        public BuildDto Build { get; set; }
    }

    class BuildTypesDto
    {
        public List<BuildTypeDto> BuildType { get; set; } = new List<BuildTypeDto>();
    }

    class TagDto
    {
        public string Name { get; set; }
    }

    class TagsDto
    {
        public List<TagDto> Tag { get; set; } = new List<TagDto>();
    }

    class TriggerBuildRequestDto
    {
        public string BranchName { get; set; }
        public bool? Personal { get; set; }
        public TriggeringOptionsDto TriggeringOptions { get; set; }
        public ParametersDto Properties { get; set; }
        public BuildTypeDto BuildType { get; set; }
        public CommentDto Comment { get; set; }
        //  TODO: lastChanges
        //    <lastChanges>
        //      <change id="modificationId"/>
        //    </lastChanges>
    }

    class TriggeringOptionsDto
    {
        public bool? CleanSources { get; set; }
        public bool? RebuildAllDependencies { get; set; }
        public bool? QueueAtTop { get; set; }
    }

    class CommentDto
    {
        public string Text { get; set; }
    }

    class TriggerDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public ParametersDto Properties { get; set; } = new ParametersDto();
    }

    class TriggersDto
    {
        public List<TriggerDto> Trigger { get; set; } = new List<TriggerDto>();
    }

    class ArtifactDependencyDto : IdDto
    {
        public string Type { get; set; }
        public bool? Disabled { get; set; } = false;
        public bool? Inherited { get; set; } = false;
        public ParametersDto Properties { get; set; } = new ParametersDto();
        public BuildTypeDto SourceBuildType { get; set; } = new BuildTypeDto();
    }

    class ArtifactDependenciesDto
    {
        public List<ArtifactDependencyDto> ArtifactDependency { get; set; } = new List<ArtifactDependencyDto>();
    }

    class ProjectDto : IdDto
    {
        public string Name { get; set; }
        public string ParentProjectId { get; set; }
        public bool? Archived { get; set; }
        public ProjectsDto Projects { get; set; } = new ProjectsDto();
        public ParametersDto Parameters { get; set; } = new ParametersDto();
        public BuildTypesDto BuildTypes { get; set; } = new BuildTypesDto();
    }

    class BuildAgentDto : IdDto
    {
        public string Name { get; set; }
        public bool? Connected { get; set; }
        public bool? Enabled { get; set; }
        public bool? Authorized { get; set; }
        public bool? Uptodate { get; set; }
        public string Ip { get; set; }
        public EnabledInfoDto EnabledInfo { get; set; }
        public AuthorizedInfoDto AuthorizedInfo { get; set; }
        public ParametersDto Properties { get; set; }
        public BuildAgentPoolDto Pool { get; set; }
        public BuildDto Build { get; set; }
    }

    class BuildAgentPoolDto : IdDto
    {
        public string Name { get; set; }
        public ProjectsDto Projects { get; set; } = new ProjectsDto();
        public BuildAgentsDto Agents { get; set; } = new BuildAgentsDto();
    }

    class ChangesDto
    {
        public List<ChangeDto> Change { get; set; } = new List<ChangeDto>();
    }

    class ChangeDto : IdDto
    {
        public string Version { get; set; }
        public UserDto User { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    class UserDto : IdDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    class ParametersDto
    {
        public List<ParameterDto> Property { get; set; } = new List<ParameterDto>();

        public ParametersDto() { }
        public ParametersDto(List<ParameterDto> properties)
        {
            Property = properties;
        }
    }

    class ParameterDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool? Own { get; set; }

        public ParameterDto(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    class PinInfoDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
    }

    class TriggeredDto
    {
        public UserDto User { get; set; }
        public BuildDto Build { get; set; }
    }

    class BuildCommentDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
    }

    class EnabledInfoCommentDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
    }

    class EnabledInfoDto
    {
        public EnabledInfoCommentDto Comment { get; set; }
    }

    class AuthorizedInfoCommentDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
    }

    class AuthorizedInfoDto
    {
        public AuthorizedInfoCommentDto Comment { get; set; }
    }

    class BuildCanceledDto
    {
        public UserDto User { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
    }

    class TriggeredBuildDto
    {
        public int? Id { get; set; }
        public string BuildTypeId { get; set; }
    }

    class RevisionsDto
    {
        public List<RevisionDto> Revision { get; set; } = new List<RevisionDto>();
    }

    class RevisionDto
    {
        public string Version { get; set; }
        public string VcsBranchName { get; set; }
        public VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    class NameValuePropertiesDto
    {
        public List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    class NameValuePropertyDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    class BuildCancelRequestDto
    {
        public string Comment { get; set; } = "";
        public bool ReaddIntoQueue { get; set; } = false;
    }

    class TestOccurrencesDto
    {
        public string NextHref { get; set; }
        public List<TestOccurrenceDto> TestOccurrence { get; set; } = new List<TestOccurrenceDto>();
    }

    class TestDto
    {
        public string Id { get; set; }
    }

    class TestOccurrenceDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public bool? Ignored { get; set; }
        public long? Duration { get; set; }
        public string IgnoreDetails { get; set; }
        public string Details { get; set; }
        public bool? CurrentlyMuted { get; set; }
        public bool? Muted { get; set; }
        public bool? NewFailure { get; set; }
        public BuildDto Build { get; set; }
        public TestDto Test { get; set; }
        public BuildDto NextFixed { get; set; }
        public BuildDto FirstFailed { get; set; }
        public const string FILTER = "testOccurrence(name,status,ignored,muted,currentlyMuted,newFailure,duration,ignoreDetails,details,firstFailed(id),nextFixed(id),build(id),test(id))";
    }

    class InvestigationListDto
    {
        public List<InvestigationDto> Investigation { get; set; } = new List<InvestigationDto>();
    }

    class InvestigationDto : IdDto
    {
        public InvestigationState? State { get; set; }
        public UserDto Assignee { get; set; }
        public AssignmentDto Assignment { get; set; }
        public InvestigationResolutionDto Resolution { get; set; }
        public InvestigationScopeDto Scope { get; set; }
        public InvestigationTargetDto Target { get; set; }
    }

    class InvestigationResolutionDto
    {
        public string Type { get; set; }
    }

    class AssignmentDto
    {
        public UserDto User { get; set; }
        public string Text { get; set; }
        public string Timestamp { get; set; }
    }

    class InvestigationTargetDto
    {
        public TestUnderInvestigationListDto Tests { get; set; }
        public ProblemUnderInvestigationListDto Problems { get; set; }
        public bool? AnyProblem { get; set; }
    }

    class TestUnderInvestigationListDto
    {
        public int? Count { get; set; }
        public List<TestDto> Test { get; set; } = new List<TestDto>();

    }

    class ProblemUnderInvestigationListDto
    {
        public int? Count { get; set; }
        public List<BuildProblemDto> Problem { get; set; } = new List<BuildProblemDto>();
    }

    class InvestigationScopeDto
    {
        public BuildTypesDto BuildTypes { get; set; }
        public ProjectDto Project { get; set; }
    }
}