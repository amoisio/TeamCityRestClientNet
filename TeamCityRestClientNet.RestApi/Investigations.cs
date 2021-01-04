using System.Collections.Generic;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.RestApi
{
    public class InvestigationListDto
    {
        public List<InvestigationDto> Investigation { get; set; } = new List<InvestigationDto>();
    }

    public class InvestigationDto : IdDto
    {
        public InvestigationState? State { get; set; }
        public UserDto Assignee { get; set; }
        public AssignmentDto Assignment { get; set; }
        public InvestigationResolutionDto Resolution { get; set; }
        public InvestigationScopeDto Scope { get; set; }
        public InvestigationTargetDto Target { get; set; }
    }

    public class AssignmentDto
    {
        public UserDto User { get; set; }
        public string Text { get; set; }
        public string Timestamp { get; set; }
    }

    public class InvestigationResolutionDto
    {
        public string Type { get; set; }
    }

    public class InvestigationScopeDto
    {
        public BuildTypeListDto BuildTypes { get; set; }
        public ProjectDto Project { get; set; }
    }

    public class InvestigationTargetDto
    {
        public TestUnderInvestigationListDto Tests { get; set; }
        public ProblemUnderInvestigationListDto Problems { get; set; }
        public bool? AnyProblem { get; set; }
    }

    public class TestUnderInvestigationListDto
    {
        public int? Count { get; set; }
        public List<TestDto> Test { get; set; } = new List<TestDto>();

    }

    public class ProblemUnderInvestigationListDto
    {
        public int? Count { get; set; }
        public List<BuildProblemDto> Problem { get; set; } = new List<BuildProblemDto>();
    }

}