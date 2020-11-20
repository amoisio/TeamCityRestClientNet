using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class BuildConfiguration : Base<BuildTypeDto>, IBuildConfiguration
    {
        public BuildConfiguration(BuildTypeDto dto, bool isFullDto, TeamCityInstance instance)
            : base(dto, isFullDto, instance)
        {

        }

        public BuildConfigurationId Id => new BuildConfigurationId(IdString);

        public string Name => NotNull(dto => dto.Name);

        public ProjectId ProjectId => new ProjectId(NotNull(dto => dto.ProjectId));

        public bool Paused => Nullable(dto => dto.Paused) ?? false;

        public List<string> BuildTags
            => Service.BuildTypeTags(IdString).GetAwaiter().GetResult()
                .Tag
                .Select(tag => tag.Name ?? throw new NullReferenceException())
                .ToList();

        public List<IFinishBuildTrigger> FinishBuildTriggers
            => Service.BuildTypeTriggers(IdString)
                .GetAwaiter()
                .GetResult()
                .Trigger
                ?.Where(trigger => trigger.Type == "buildDependencyTrigger")
                ?.Select(trigger => new FinishBuildTrigger(trigger))
                .ToList<IFinishBuildTrigger>()
                ?? new List<IFinishBuildTrigger>();

        public List<IArtifactDependency> ArtifactDependencies
            => Service.BuildTypeArtifactDependencies(IdString)
                .GetAwaiter()
                .GetResult()
                .ArtifactDependency
                ?.Where(dep => dep.Disabled == false)
                ?.Select(dep => new ArtifactDependency(dep, true, Instance))
                .ToList<IArtifactDependency>()
                ?? new List<IArtifactDependency>();

        public int BuildCounter
        {
            get
            {
                var setting = GetSetting("buildNumberCounter");
                if (int.TryParse(setting, out int counter))
                    return counter;
                else
                    throw new TeamCityQueryException($"Cannot get 'buildNumberCounter' setting for {IdString}");
            }
            set
            {
                //         LOG.info("Setting build counter to '$value' in BuildConfigurationId=$idString")
                Service.SetBuildTypeSettings(IdString, "buildNumberCounter", value.ToString()).GetAwaiter().GetResult();
            }
        }

        public string BuildNumberFormat
        {
            get
            {
                return GetSetting("buildNumberPattern")
                    ?? throw new TeamCityQueryException($"Cannot get 'buildNumberPattern' setting for {IdString}");
            }
            set
            {
                //         LOG.info("Setting build number format to '$value' in BuildConfigurationId=$idString")
                Service.SetBuildTypeSettings(IdString, "buildNumberPattern", value).GetAwaiter().GetResult();
            }
        }

        private string GetSetting(string settingName)
            => Nullable(dto => dto.Settings)
                ?.Property
                ?.FirstOrDefault(prop => prop.Name == settingName)
                ?.Value;

        public string GetHomeUrl(string branch = null)
            => Instance.GetUserUrlPage(
                "viewType.html",
                buildTypeId: Id,
                branch: branch);

        public IBuild RunBuild(
            IDictionary<string, string> parameters = null, 
            bool queueAtTop = false, 
            bool? cleanSources = null, 
            bool rebuildAllDependencies = false, 
            string comment = null, 
            string logicalBranchName = null, 
            bool personal = false)
        {
            var request = new TriggerBuildRequestDto();
            request.BuildType = new BuildTypeDto { Id = this.IdString };
            request.BranchName = logicalBranchName;
            request.Comment = comment != null ? new CommentDto { Text = comment } : null;
            request.Personal = personal;
            request.TriggeringOptions = new TriggeringOptionsDto {
                CleanSources = cleanSources,
                RebuildAllDependencies = rebuildAllDependencies,
                QueueAtTop = queueAtTop
            };
            request.Properties = parameters != null
                ? new ParametersDto(parameters.Select(par => new ParameterDto(par.Key, par.Value)).ToList())
                : null;

            var triggeredBuildDto = Service.TriggerBuild(request).GetAwaiter().GetResult();
            return Instance.Build(
                    new BuildId(
                        triggeredBuildDto.Id?.ToString() 
                        ?? throw new NullReferenceException()));
        }
        public void SetParameter(string name, string value)
        {
            //  LOG.info("Setting parameter $name=$value in BuildConfigurationId=$idString")
            Service.SetBuildTypeParameter(IdString, name, value).GetAwaiter().GetResult();
        }

        public override string ToString()
            => $"BuildConfiguration(id={IdString},name={Name})";
        protected override async Task<BuildTypeDto> FetchFullDto()
            => await Service.BuildConfiguration(IdString);
    }
}