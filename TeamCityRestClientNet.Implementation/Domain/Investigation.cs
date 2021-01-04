using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using System.Linq;

namespace TeamCityRestClientNet.Domain
{
    class Investigation : Base<InvestigationDto>, IInvestigation
    {
        private Investigation(InvestigationDto fullDto, TeamCityServer instance)
            : base(fullDto, instance)
        {
            Assignee = new AsyncLazy<IUser>(async () =>
                await User.Create(fullDto.Assignee.Id, instance).ConfigureAwait(false));

            Reporter = new AsyncLazy<IUser>(async () =>
            {
                if (Dto.Assignment == null || Dto.Assignment.User == null) return null;
                return await User.Create(Dto.Assignment.User.Id, instance).ConfigureAwait(false);
            });

            Scope = new AsyncLazy<IInvestigationScope>(async () =>
            {
                var scope = fullDto.Scope;
                var project = scope.Project != null
                    ? await Project.Create(scope.Project, false, instance).ConfigureAwait(false)
                    : null;

                if (project != null)
                    return new InProject(project);

                /* neither teamcity.jetbrains nor buildserver contain more then one assignment build type */
                if (scope.BuildTypes?.Items != null && scope.BuildTypes.Items.Count > 1)
                {
                    throw new Exception("more then one buildType");
                }

                var buildType = scope.BuildTypes != null
                    ? await BuildType.Create(scope.BuildTypes.Items[0].Id, instance).ConfigureAwait(false)
                    : null;

                if (buildType != null)
                {
                    return new InBuildType(buildType);
                }

                throw new Exception("scope is missed in the bean");
            });
        }

        public static async Task<IInvestigation> Create(InvestigationDto dto, bool isFullDto, TeamCityServer instance)
        {
            var fullDto = isFullDto
                ? dto
                : await instance.Service.Investigation(dto.Id).ConfigureAwait(false);
            return new Investigation(dto, instance);
        }

        public InvestigationId Id => new InvestigationId(IdString);
        public InvestigationState State => Dto.State ?? throw new NullReferenceException();
        public AsyncLazy<IUser> Assignee { get; }
        public AsyncLazy<IUser> Reporter { get; }
        public string Comment => Dto.Assignment?.Text ?? String.Empty;
        public InvestigationResolveMethod ResolveMethod
        {
            get
            {
                var asString = Dto.Resolution.Type;
                if (asString == "whenFixed")
                    return InvestigationResolveMethod.WHEN_FIXED;
                else if (asString == "manually")
                    return InvestigationResolveMethod.MANUALLY;
                else
                    throw new Exception("Properties are invalid");
            }
        }

        public InvestigationTargetType TargetType
        {
            get
            {
                var target = Dto.Target;
                if (target.Tests != null)
                    return InvestigationTargetType.TEST;
                if (target.Problems != null)
                    return InvestigationTargetType.BUILD_PROBLEM;
                else
                    return InvestigationTargetType.BUILD_CONFIGURATION;
            }
        }

        public List<Id> TestIds
            => Dto.Target?.Tests?.Test.Select(t => new Id(t.Id)).ToList();

        public List<BuildProblemId> ProblemIds
            => Dto.Target?.Problems?.Problem?.Select(p => new BuildProblemId(p.Id)).ToList();

        public AsyncLazy<IInvestigationScope> Scope { get; }

        public override string ToString()
            => $"Investigation(id={IdString},state={State})";
    }
}