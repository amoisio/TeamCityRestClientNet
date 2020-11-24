using System.Collections.Generic;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Service
{
    class ProjectsDto
    {
        internal List<ProjectDto> Project { get; set; } = new List<ProjectDto>();
    }

    class BuildAgentsDto
    {
        internal List<BuildAgentDto> Agent { get; set; } = new List<BuildAgentDto>();
    }

    class BuildAgentPoolsDto
    {
        internal List<BuildAgentPoolDto> AgentPool { get; set; } = new List<BuildAgentPoolDto>();
    }

    class ArtifactFileListDto
    {
        internal List<ArtifactFileDto> File { get; set; } = new List<ArtifactFileDto>();
    }

    class ArtifactFileDto
    {
        internal string Name { get; set; }
        internal string FullName { get; set; }
        internal long? Size { get; set; }
        internal string ModificationTime { get; set; }

        internal const string FIELDS = "${ArtifactFileBean::fullName.name},${ArtifactFileBean::name.name},${ArtifactFileBean::size.name},${ArtifactFileBean::modificationTime.name}";
    }

    class IdDto
    {
        internal string Id { get; set; }
    }

    class VcsRootListDto
    {
        internal string NextHref { get; set; }
        internal List<VcsRootDto> VcsRoot { get; set; } = new List<VcsRootDto>();
    }

    class VcsRootDto : IdDto
    {
        internal string Name { get; set; }

        internal NameValuePropertiesDto Properties { get; set; }
    }

    class VcsRootInstanceDto
    {
        internal string VcsRootId { get; set; }
        internal string Name { get; set; }
    }

    class BuildListDto
    {
        internal string NextHref { get; set; }
        internal List<BuildDto> Build { get; set; } = new List<BuildDto>();
    }

    class UserListDto
    {
        internal List<UserDto> User { get; set; } = new List<UserDto>();
    }

    class BuildDto : IdDto
    {
        internal string BuildTypeId { get; set; }
        internal BuildCanceledDto CanceledInfo { get; set; }
        internal string Number { get; set; }
        internal BuildStatus? Status { get; set; }
        internal string State { get; set; }
        internal bool? Personal { get; set; }
        internal string BranchName { get; set; }
        internal bool? DefaultBranch { get; set; }
        internal bool? Composite { get; set; }

        internal string StatusText { get; set; }
        internal string QueuedDate { get; set; }
        internal string StartDate { get; set; }
        internal string FinishDate { get; set; }

        internal TagsDto Tags { get; set; }
        internal BuildRunningInfoDto RunningInfo { get; set; }
        internal RevisionsDto Revisions { get; set; }

        internal PinInfoDto PinInfo { get; set; }

        internal TriggeredDto Triggered { get; set; }
        internal BuildCommentDto Comment { get; set; }
        internal BuildAgentDto Agent { get; set; }

        internal ParametersDto Properties { get; set; } = new ParametersDto();
        internal BuildTypeDto BuildType { get; set; } = new BuildTypeDto();

        internal BuildListDto SnapshotDependencies { get; set; }
    }

    class BuildRunningInfoDto
    {
        internal int PercentageComplete { get; set; } = 0;
        internal long ElapsedSeconds { get; set; } = 0;
        internal long EstimatedTotalSeconds { get; set; } = 0;
        internal bool Outdated { get; set; } = false;
        internal bool ProbablyHanging { get; set; } = false;
    }

    class BuildTypeDto : IdDto
    {
        internal string Name { get; set; }
        internal string ProjectId { get; set; }
        internal bool? Paused { get; set; }
        internal BuildTypeSettingsDto Settings { get; set; }
    }

    class BuildTypeSettingsDto
    {
        internal List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    class BuildProblemDto
    {
        internal string Id { get; set; }
        internal string Type { get; set; }
        internal string Identity { get; set; }
    }

    class BuildProblemOccurrencesDto
    {
        internal string NextHref { get; set; }
        internal List<BuildProblemOccurrenceDto> ProblemOccurrence { get; set; } = new List<BuildProblemOccurrenceDto>();
    }

    class BuildProblemOccurrenceDto
    {
        internal string Details { get; set; }
        internal string AdditionalData { get; set; }
        internal BuildProblemDto Problem { get; set; }
        internal BuildDto Build { get; set; }
    }

    class BuildTypesDto
    {
        internal List<BuildTypeDto> BuildType { get; set; } = new List<BuildTypeDto>();
    }

    class TagDto
    {
        internal string Name { get; set; }
    }

    class TagsDto
    {
        internal List<TagDto> Tag { get; set; } = new List<TagDto>();
    }

    class TriggerBuildRequestDto
    {
        internal string BranchName { get; set; }
        internal bool? Personal { get; set; }
        internal TriggeringOptionsDto TriggeringOptions { get; set; }

        internal ParametersDto Properties { get; set; }
        internal BuildTypeDto BuildType { get; set; }
        internal CommentDto Comment { get; set; }

        //  TODO: lastChanges
        //    <lastChanges>
        //      <change id="modificationId"/>
        //    </lastChanges>
    }

    class TriggeringOptionsDto
    {
        internal bool? CleanSources { get; set; }
        internal bool? RebuildAllDependencies { get; set; }
        internal bool? QueueAtTop { get; set; }
    }

    class CommentDto
    {
        internal string Text { get; set; }
    }

    class TriggerDto
    {
        internal string Id { get; set; }
        internal string Type { get; set; }
        internal ParametersDto Properties { get; set; } = new ParametersDto();
    }

    class TriggersDto
    {
        internal List<TriggerDto> Trigger { get; set; } = new List<TriggerDto>();
    }

    class ArtifactDependencyDto : IdDto
    {
        internal string Type { get; set; }
        internal bool? Disabled { get; set; } = false;
        internal bool? Inherited { get; set; } = false;
        internal ParametersDto Properties { get; set; } = new ParametersDto();
        internal BuildTypeDto SourceBuildType { get; set; } = new BuildTypeDto();
    }

    class ArtifactDependenciesDto
    {
        internal List<ArtifactDependencyDto> ArtifactDependency { get; set; } = new List<ArtifactDependencyDto>();
    }

    class ProjectDto : IdDto
    {
        internal string Name { get; set; }
        internal string ParentProjectId { get; set; }
        internal bool? Archived { get; set; }

        internal ProjectsDto Projects { get; set; } = new ProjectsDto();
        internal ParametersDto Parameters { get; set; } = new ParametersDto();
        internal BuildTypesDto BuildTypes { get; set; } = new BuildTypesDto();
    }

    class BuildAgentDto : IdDto
    {
        internal string Name { get; set; }
        internal bool? Connected { get; set; }
        internal bool? Enabled { get; set; }
        internal bool? Authorized { get; set; }
        internal bool? Uptodate { get; set; }
        internal string Ip { get; set; }

        internal EnabledInfoDto EnabledInfo { get; set; }
        internal AuthorizedInfoDto AuthorizedInfo { get; set; }

        internal ParametersDto Properties { get; set; }
        internal BuildAgentPoolDto Pool { get; set; }
        internal BuildDto Build { get; set; }
    }

    class BuildAgentPoolDto : IdDto
    {
        internal string Name { get; set; }

        internal ProjectsDto Projects { get; set; } = new ProjectsDto();
        internal BuildAgentsDto Agents { get; set; } = new BuildAgentsDto();
    }

    class ChangesDto
    {
        internal List<ChangeDto> Change { get; set; } = new List<ChangeDto>();
    }

    class ChangeDto : IdDto
    {
        internal string Version { get; set; }
        internal UserDto User { get; set; }
        internal string Date { get; set; }
        internal string Comment { get; set; }
        internal string Username { get; set; }
        internal VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    class UserDto : IdDto
    {
        internal string Username { get; set; }
        internal string Name { get; set; }
        internal string Email { get; set; }
    }

    class ParametersDto
    {
        internal List<ParameterDto> Property { get; set; } = new List<ParameterDto>();

        internal ParametersDto()
        {


        }

        internal ParametersDto(List<ParameterDto> properties)
        {
            Property = properties;
        }
    }

    class ParameterDto
    {
        internal string Name { get; set; }
        internal string Value { get; set; }
        internal bool? Own { get; set; }

        internal ParameterDto(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    class PinInfoDto
    {
        internal UserDto User { get; set; }
        internal string Timestamp { get; set; }
    }

    class TriggeredDto
    {
        internal UserDto User { get; set; }
        internal BuildDto Build { get; set; }
    }

    class BuildCommentDto
    {
        internal UserDto User { get; set; }
        internal string Timestamp { get; set; }
        internal string Text { get; set; }
    }

    class EnabledInfoCommentDto
    {
        internal UserDto User { get; set; }
        internal string Timestamp { get; set; }
        internal string Text { get; set; }
    }

    class EnabledInfoDto
    {
        internal EnabledInfoCommentDto Comment { get; set; }
    }

    class AuthorizedInfoCommentDto
    {
        internal UserDto User { get; set; }
        internal string Timestamp { get; set; }
        internal string Text { get; set; }
    }

    class AuthorizedInfoDto
    {
        internal AuthorizedInfoCommentDto Comment { get; set; }
    }

    class BuildCanceledDto
    {
        internal UserDto User { get; set; }
        internal string Timestamp { get; set; }
        internal string Text { get; set; }
    }

    class TriggeredBuildDto
    {
        internal int? Id { get; set; }
        internal string BuildTypeId { get; set; }
    }

    class RevisionsDto
    {
        internal List<RevisionDto> Revision { get; set; } = new List<RevisionDto>();
    }

    class RevisionDto
    {
        internal string Version { get; set; }
        internal string VcsBranchName { get; set; }
        internal VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    class NameValuePropertiesDto
    {
        internal List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    class NameValuePropertyDto
    {
        internal string Name { get; set; }
        internal string Value { get; set; }
    }

    class BuildCancelRequestDto
    {
        internal string Comment { get; set; } = "";
        internal bool ReaddIntoQueue { get; set; } = false;
    }

    class TestOccurrencesDto
    {
        internal string NextHref { get; set; }
        internal List<TestOccurrenceDto> TestOccurrence { get; set; } = new List<TestOccurrenceDto>();
    }

    class TestDto
    {
        internal string Id { get; set; }
    }

    class TestOccurrenceDto
    {
        internal string Name { get; set; }
        internal string Status { get; set; }
        internal bool? Ignored { get; set; }
        internal long? Duration { get; set; }
        internal string IgnoreDetails { get; set; }
        internal string Details { get; set; }
        internal bool? CurrentlyMuted { get; set; }
        internal bool? Muted { get; set; }
        internal bool? NewFailure { get; set; }

        internal BuildDto Build { get; set; }
        internal TestDto Test { get; set; }
        internal BuildDto NextFixed { get; set; }
        internal BuildDto FirstFailed { get; set; }

        internal const string FILTER = "testOccurrence(name,status,ignored,muted,currentlyMuted,newFailure,duration,ignoreDetails,details,firstFailed(id),nextFixed(id),build(id),test(id))";
    }

    class InvestigationListDto
    {
        internal List<InvestigationDto> Investigation { get; set; } = new List<InvestigationDto>();
    }

    class InvestigationDto : IdDto
    {
        internal InvestigationState? State { get; set; }
        internal UserDto Assignee { get; set; }
        internal AssignmentDto Assignment { get; set; }
        internal InvestigationResolutionDto Resolution { get; set; }
        internal InvestigationScopeDto Scope { get; set; }
        internal InvestigationTargetDto Target { get; set; }
    }

    class InvestigationResolutionDto
    {
        internal string Type { get; set; }
    }

    class AssignmentDto
    {
        internal UserDto User { get; set; }
        internal string Text { get; set; }
        internal string Timestamp { get; set; }
    }

    class InvestigationTargetDto
    {
        internal TestUnderInvestigationListDto Tests { get; set; }
        internal ProblemUnderInvestigationListDto Problems { get; set; }
        internal bool? AnyProblem { get; set; }
    }

    class TestUnderInvestigationListDto
    {
        internal int? Count { get; set; }
        internal List<TestDto> Test { get; set; } = new List<TestDto>();

    }

    class ProblemUnderInvestigationListDto
    {
        internal int? Count { get; set; }
        internal List<BuildProblemDto> Problem { get; set; } = new List<BuildProblemDto>();
    }

    class InvestigationScopeDto
    {
        internal BuildTypesDto BuildTypes { get; set; }
        internal ProjectDto Project { get; set; }
    }
}