using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nito.AsyncEx;

using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class BuildConfiguration : Base<BuildTypeDto>, IBuildConfiguration
    {
        private BuildConfiguration(BuildTypeDto dto, TeamCityServer instance)
            : base(dto, instance)
        {
            this.BuildTags = new AsyncLazy<List<string>>(async ()
                => (await Service.BuildTypeTags(IdString).ConfigureAwait(false))
                    .Tag
                    .Select(tag => tag.Name ?? throw new NullReferenceException())
                    .ToList());

            this.FinishBuildTriggers = new AsyncLazy<List<IFinishBuildTrigger>>(async ()
                => (await Service.BuildTypeTriggers(IdString).ConfigureAwait(false))
                    .Trigger
                    ?.Where(trigger => trigger.Type == "buildDependencyTrigger")
                    ?.Select(trigger => new FinishBuildTrigger(trigger))
                    .ToList<IFinishBuildTrigger>()
                    ?? new List<IFinishBuildTrigger>());

            this.ArtifactDependencies = new AsyncLazy<List<IArtifactDependency>>(async ()
                => (await Service.BuildTypeArtifactDependencies(IdString).ConfigureAwait(false))
                    .ArtifactDependency
                    ?.Where(dep => dep.Disabled == false)
                    ?.Select(dep => new ArtifactDependency(dep, Instance))
                    .ToList<IArtifactDependency>()
                    ?? new List<IArtifactDependency>());
        }

        public static async Task<IBuildConfiguration> Create(string idString, TeamCityServer instance)
        {
            var dto = await instance.Service.BuildConfiguration(idString).ConfigureAwait(false);
            return new BuildConfiguration(dto, instance);
        }

        public BuildConfigurationId Id => new BuildConfigurationId(IdString);
        public string Name => NotNull(dto => dto.Name);
        public ProjectId ProjectId => new ProjectId(NotNull(dto => dto.ProjectId));
        public bool Paused => this.Dto.Paused ?? false;
        public AsyncLazy<List<string>> BuildTags { get; }
        public AsyncLazy<List<IFinishBuildTrigger>> FinishBuildTriggers { get; }
        public AsyncLazy<List<IArtifactDependency>> ArtifactDependencies { get; }
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
        }

        public async Task SetBuildCounter(int count)
            => await SetParameter("buildNumberCounter", count).ConfigureAwait(false);

        public string BuildNumberFormat
        {
            get
            {
                return GetSetting("buildNumberPattern")
                    ?? throw new TeamCityQueryException($"Cannot get 'buildNumberPattern' setting for {IdString}");
            }
        }

        public async Task SetBuildNumberFormat(string format)
            => await SetParameter("buildNumberPattern", format).ConfigureAwait(false);

        private string GetSetting(string settingName)
            => this.Dto.Settings
                ?.Property
                ?.FirstOrDefault(prop => prop.Name == settingName)
                ?.Value;

        public string GetHomeUrl(string branch = null)
            => Instance.GetUserUrlPage(
                "viewType.html",
                buildTypeId: Id,
                branch: branch);

        public async Task<IBuild> RunBuild(
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
            request.TriggeringOptions = new TriggeringOptionsDto
            {
                CleanSources = cleanSources,
                RebuildAllDependencies = rebuildAllDependencies,
                QueueAtTop = queueAtTop
            };
            request.Properties = parameters != null
                ? new ParametersDto(parameters.Select(par => new ParameterDto(par.Key, par.Value)).ToList())
                : null;

            var triggeredBuildDto = await Service.TriggerBuild(request).ConfigureAwait(false);
            return await Instance.Build(
                new BuildId(
                    triggeredBuildDto.Id?.ToString()
                    ?? throw new NullReferenceException())).ConfigureAwait(false);
        }

        public async Task SetParameter<T>(string name, T value)
        {
            var valueStr = value.ToString();
            //  LOG.info("Setting parameter $name=$value in BuildConfigurationId=$idString")
            await Service.SetBuildTypeParameter(IdString, name, valueStr).ConfigureAwait(false);
            SetSetting(name, valueStr);
        }

        private void SetSetting(string settingName, string value)
        {
            var setting = this.Dto.Settings
                ?.Property
                ?.FirstOrDefault(prop => prop.Name == settingName);
            if (setting != null)
                setting.Value = value;
        }

        public override string ToString()
            => $"BuildConfiguration(id={IdString},name={Name})";
    }
}