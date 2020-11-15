using System.Collections.Generic;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Service
{
    internal class ProjectsDto
    {
        List<ProjectDto> Project { get; set; } = new List<ProjectDto>();
    }

    internal class BuildAgentsDto
    {
        List<BuildAgentDto> Agent { get; set; } = new List<BuildAgentDto>();
    }

    internal class BuildAgentPoolsDto
    {
        List<BuildAgentPoolDto> AgentPool { get; set; } = new List<BuildAgentPoolDto>();
    }

    internal class ArtifactFileListDto
    {
        List<ArtifactFileDto> File { get; set; } = new List<ArtifactFileDto>();
    }

    internal class ArtifactFileDto
    {
        string Name { get; set; }
        string FullName { get; set; }
        long? Size { get; set; }
        string ModificationTime { get; set; }

        const string FIELDS = "${ArtifactFileBean::fullName.name},${ArtifactFileBean::name.name},${ArtifactFileBean::size.name},${ArtifactFileBean::modificationTime.name}";
    }

    internal class IdDto
    {
        string Id { get; set; }
    }

    internal class VcsRootListDto
    {
        string NextHref { get; set; }
        List<VcsRootDto> VcsRoot { get; set; } = new List<VcsRootDto>();
    }

    internal class VcsRootDto : IdDto
    {
        string Name { get; set; }

        NameValuePropertiesDto Properties { get; set; }
    }

    internal class VcsRootInstanceDto
    {
        string VcsRootId { get; set; }
        string Name { get; set; }
    }

    internal class BuildListDto
    {
        string NextHref { get; set; }
        List<BuildDto> Build { get; set; } = new List<BuildDto>();
    }

    internal class UserListDto
    {
        List<UserDto> User { get; set; } = new List<UserDto>();
    }

    internal class BuildDto : IdDto
    {
        string BuildTypeId { get; set; }
        BuildCanceledDto CanceledInfo { get; set; }
        string Number { get; set; }
        BuildStatus? Status { get; set; }
        string State { get; set; }
        bool? Personal { get; set; }
        string BranchName { get; set; }
        bool? DefaultBranch { get; set; }
        bool? Composite { get; set; }

        string StatusText { get; set; }
        string QueuedDate { get; set; }
        string StartDate { get; set; }
        string FinishDate { get; set; }

        TagsDto Tags { get; set; }
        BuildRunningInfoDto RunningInfo { get; set; }
        RevisionsDto Revisions { get; set; }

        PinInfoDto PinInfo { get; set; }

        TriggeredDto Triggered { get; set; }
        BuildCommentDto Comment { get; set; }
        BuildAgentDto Agent { get; set; }

        ParametersDto Properties { get; set; } = new ParametersDto();
        BuildTypeDto BuildType { get; set; } = new BuildTypeDto();

        BuildListDto SnapshotDependencies { get; set; }
    }

    internal class BuildRunningInfoDto
    {
        int PercentageComplete { get; set; } = 0;
        long ElapsedSeconds { get; set; } = 0;
        long EstimatedTotalSeconds { get; set; } = 0;
        bool Outdated { get; set; } = false;
        bool ProbablyHanging { get; set; } = false;
    }

    internal class BuildTypeDto : IdDto
    {
        string Name { get; set; }
        string ProjectId { get; set; }
        bool? Paused { get; set; }
        BuildTypeSettingsDto Settings { get; set; }
    }

    internal class BuildTypeSettingsDto
    {
        List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    internal class BuildProblemDto
    {
        string Id { get; set; }
        string Type { get; set; }
        string Identity { get; set; }
    }

    internal class BuildProblemOccurrencesDto
    {
        string NextHref { get; set; }
        List<BuildProblemOccurrenceDto> ProblemOccurrence { get; set; } = new List<BuildProblemOccurrenceDto>();
    }

    internal class BuildProblemOccurrenceDto
    {
        string Details { get; set; }
        string AdditionalData { get; set; }
        BuildProblemDto Problem { get; set; }
        BuildDto Build { get; set; }
    }

    internal class BuildTypesDto
    {
        List<BuildTypeDto> BuildType { get; set; } = new List<BuildTypeDto>();
    }

    internal class TagDto
    {
        string Name { get; set; }
    }

    internal class TagsDto
    {
        List<TagDto> Tag { get; set; } = new List<TagDto>();
    }

    internal class TriggerBuildRequestDto
    {
        string BranchName { get; set; }
        bool? Personal { get; set; }
        TriggeringOptionsDto TriggeringOptions { get; set; }

        ParametersDto Properties { get; set; }
        BuildTypeDto BuildType { get; set; }
        CommentDto Comment { get; set; }

        //  TODO: lastChanges
        //    <lastChanges>
        //      <change id="modificationId"/>
        //    </lastChanges>
    }

    internal class TriggeringOptionsDto
    {
        bool? CleanSources { get; set; }
        bool? RebuildAllDependencies { get; set; }
        bool? QueueAtTop { get; set; }
    }

    internal class CommentDto
    {
        string Text { get; set; }
    }

    internal class TriggerDto
    {
        string Id { get; set; }
        string Type { get; set; }
        ParametersDto Properties { get; set; } = new ParametersDto();
    }

    internal class TriggersDto
    {
        List<TriggerDto> Trigger { get; set; } = new List<TriggerDto>();
    }

    internal class ArtifactDependencyDto : IdDto
    {
        string Type { get; set; }
        bool? Disabled { get; set; } = false;
        bool? Inherited { get; set; } = false;
        ParametersDto Properties { get; set; } = new ParametersDto();
        BuildTypeDto SourceBuildType { get; set; } = new BuildTypeDto();
    }

    internal class ArtifactDependenciesDto
    {
        List<ArtifactDependencyDto> ArtifactDependency { get; set; } = new List<ArtifactDependencyDto>();
    }

    internal class ProjectDto : IdDto
    {
        string Name { get; set; }
        string ParentProjectId { get; set; }
        bool? Archived { get; set; }

        ProjectsDto Projects { get; set; } = new ProjectsDto();
        ParametersDto Parameters { get; set; } = new ParametersDto();
        BuildTypesDto BuildTypes { get; set; } = new BuildTypesDto();
    }

    internal class BuildAgentDto : IdDto
    {
        string Name { get; set; }
        bool? Connected { get; set; }
        bool? Enabled { get; set; }
        bool? Authorized { get; set; }
        bool? Uptodate { get; set; }
        string Ip { get; set; }

        EnabledInfoDto EnabledInfo { get; set; }
        AuthorizedInfoDto AuthorizedInfo { get; set; }

        ParametersDto Properties { get; set; }
        BuildAgentPoolDto Pool { get; set; }
        BuildDto Build { get; set; }
    }

    internal class BuildAgentPoolDto : IdDto
    {
        string Name { get; set; }

        ProjectsDto Projects { get; set; } = new ProjectsDto();
        BuildAgentsDto Agents { get; set; } = new BuildAgentsDto();
    }

    internal class ChangesDto
    {
        List<ChangeDto> Change { get; set; } = new List<ChangeDto>();
    }

    internal class ChangeDto : IdDto
    {
        string Version { get; set; }
        UserDto User { get; set; }
        string Date { get; set; }
        string Comment { get; set; }
        string Username { get; set; }
        VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    internal class UserDto : IdDto
    {
        string Username { get; set; }
        string Name { get; set; }
        string Email { get; set; }
    }

    internal class ParametersDto
    {
        List<ParameterDto> Property { get; set; } = new List<ParameterDto>();

        public ParametersDto()
        {

        }

        public ParametersDto(List<ParameterDto> properties)
        {
            Property = properties;
        }
    }

    internal class ParameterDto
    {
        string Name { get; set; }
        string Value { get; set; }
        bool? Own { get; set; }

        ParameterDto(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    internal class PinInfoDto
    {
        UserDto User { get; set; }
        string Timestamp { get; set; }
    }

    internal class TriggeredDto
    {
        UserDto User { get; set; }
        BuildDto Build { get; set; }
    }

    internal class BuildCommentDto
    {
        UserDto User { get; set; }
        string Timestamp { get; set; }
        string Text { get; set; }
    }

    internal class EnabledInfoCommentDto
    {
        UserDto User { get; set; }
        string Timestamp { get; set; }
        string Text { get; set; }
    }

    internal class EnabledInfoDto
    {
        EnabledInfoCommentDto Comment { get; set; }
    }

    internal class AuthorizedInfoCommentDto
    {
        UserDto User { get; set; }
        string Timestamp { get; set; }
        string Text { get; set; }
    }

    internal class AuthorizedInfoDto
    {
        AuthorizedInfoCommentDto Comment { get; set; }
    }

    internal class BuildCanceledDto
    {
        UserDto User { get; set; }
        string Timestamp { get; set; }
        string Text { get; set; }
    }

    internal class TriggeredBuildDto
    {
        int? Id { get; set; }
        string BuildTypeId { get; set; }
    }

    internal class RevisionsDto
    {
        List<RevisionDto> Revision { get; set; } = new List<RevisionDto>();
    }

    internal class RevisionDto
    {
        string Version { get; set; }
        string VcsBranchName { get; set; }
        VcsRootInstanceDto VcsRootInstance { get; set; }
    }

    internal class NameValuePropertiesDto
    {
        List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    internal class NameValuePropertyDto
    {
        string Name { get; set; }
        string Value { get; set; }
    }

    internal class BuildCancelRequestDto
    {
        string Comment { get; set; } = "";
        bool ReaddIntoQueue { get; set; } = false;
    }

    internal class TestOccurrencesDto
    {
        string NextHref { get; set; }
        List<TestOccurrenceDto> TestOccurrence { get; set; } = new List<TestOccurrenceDto>();
    }

    internal class TestDto
    {
        string Id { get; set; }
    }

    internal class TestOccurrenceDto
    {
        string Name { get; set; }
        string Status { get; set; }
        bool? Ignored { get; set; }
        long? Duration { get; set; }
        string IgnoreDetails { get; set; }
        string Details { get; set; }
        bool? CurrentlyMuted { get; set; }
        bool? Muted { get; set; }
        bool? NewFailure { get; set; }

        BuildDto Build { get; set; }
        TestDto Test { get; set; }
        BuildDto NextFixed { get; set; }
        BuildDto FirstFailed { get; set; }

        const string FILTER = "testOccurrence(name,status,ignored,muted,currentlyMuted,newFailure,duration,ignoreDetails,details,firstFailed(id),nextFixed(id),build(id),test(id))";
    }

    internal class InvestigationListDto
    {
        List<InvestigationDto> Investigation { get; set; } = new List<InvestigationDto>();
    }

    internal class InvestigationDto : IdDto
    {
        InvestigationState? State { get; set; }
        UserDto Assignee { get; set; }
        AssignmentDto Assignment { get; set; }
        InvestigationResolutionDto Resolution { get; set; }
        InvestigationScopeDto Scope { get; set; }
        InvestigationTargetDto Target { get; set; }
    }

    internal class InvestigationResolutionDto
    {
        string Type { get; set; }
    }

    internal class AssignmentDto
    {
        UserDto User { get; set; }
        string Text { get; set; }
        string Timestamp { get; set; }
    }

    internal class InvestigationTargetDto
    {
        TestUnderInvestigationListDto Tests { get; set; }
        ProblemUnderInvestigationListDto Problems { get; set; }
        bool? AnyProblem { get; set; }
    }

    internal class TestUnderInvestigationListDto
    {
        int? Count { get; set; }
        List<TestDto> Test { get; set; } = new List<TestDto>();

    }

    internal class ProblemUnderInvestigationListDto
    {
        int? Count { get; set; }
        List<BuildProblemDto> Problem { get; set; } = new List<BuildProblemDto>();
    }

    internal class InvestigationScopeDto
    {
        BuildTypesDto BuildTypes { get; set; }
        ProjectDto Project { get; set; }
    }
}