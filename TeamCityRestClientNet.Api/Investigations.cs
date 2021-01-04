using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IInvestigation : IIdentifiable
    {
        InvestigationState State { get; }
        AsyncLazy<IUser> Assignee { get; }
        AsyncLazy<IUser> Reporter { get; }
        string Comment { get; }
        InvestigationResolveMethod ResolveMethod { get; }
        InvestigationTargetType TargetType { get; }
        List<Id> TestIds { get; }
        List<BuildProblemId> ProblemIds { get; }
        AsyncLazy<IInvestigationScope> Scope { get; }
    }

    public interface IInvestigationLocator : ILocator<IInvestigation>
    {
        IInvestigationLocator LimitResults(int count);
        IInvestigationLocator ForProject(Id projectId);
        IInvestigationLocator WithTargetType(InvestigationTargetType targetType);
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

    public class InBuildType : IInvestigationScope
    {
        private readonly IBuildType configuration;
        public InBuildType(IBuildType configuration)
        {
            this.configuration = configuration;
        }
    }
}