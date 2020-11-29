using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public struct InvestigationId
    {
        public InvestigationId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface IInvestigation
    {
        InvestigationId Id { get; }
        InvestigationState State { get; }
        AsyncLazy<IUser> Assignee { get; }
        AsyncLazy<IUser> Reporter { get; }
        string Comment { get; }
        InvestigationResolveMethod ResolveMethod { get; }
        InvestigationTargetType TargetType { get; }
        List<TestId> TestIds { get; }
        List<BuildProblemId> ProblemIds { get; }
        AsyncLazy<IInvestigationScope> Scope { get; }
    }

    public interface IInvestigationLocator
    {
        IInvestigationLocator LimitResults(int count);
        IInvestigationLocator ForProject(ProjectId projectId);
        IInvestigationLocator WithTargetType(InvestigationTargetType targetType);
        Task<IEnumerable<IInvestigation>> All();
    }

    public enum InvestigationState
    {
        TAKEN,
        FIXED,
        GIVEN_UP
    }

    public enum InvestigationResolveMethod
    {
        MANUALLY,
        WHEN_FIXED
    }

    public enum InvestigationTargetType
    {
        TEST,
        BUILD_PROBLEM,
        BUILD_CONFIGURATION
    }

    public interface IInvestigationScope { }

    public class InProject : IInvestigationScope
    {
        private readonly IProject project;
        public InProject(IProject project)
        {
            this.project = project;
        }
    }

    public class InBuildConfiguration : IInvestigationScope
    {
        private readonly IBuildConfiguration configuration;
        public InBuildConfiguration(IBuildConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}