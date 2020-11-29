using System;
using System.Collections.Generic;
using System.Linq;
using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class ArtifactDependency : Base<ArtifactDependencyDto>, IArtifactDependency
    {
        public ArtifactDependency(ArtifactDependencyDto fullDto, TeamCityServer instance)
            : base(fullDto, instance) 
        { 
            this.DependsOnBuildConfiguration = new AsyncLazy<IBuildConfiguration>(async () 
                => await BuildConfiguration.Create(
                    NotNull(dto => dto.SourceBuildType.Id), 
                    Instance)
                    .ConfigureAwait(false));
        }

        private string DependentBuildConfigurationId 
            => this.Dto.SourceBuildType.Id;
        public AsyncLazy<IBuildConfiguration> DependsOnBuildConfiguration { get; }

        public string Branch => FindPropertyByName("revisionBranch");

        public List<IArtifactRule> ArtifactRules
            => FindPropertyByName("pathRules")
                ?.Split(" ")
                .Select(rule => new ArtifactRule(rule))
                .ToList<IArtifactRule>()
                ?? throw new NullReferenceException();

        public bool CleanDestinationDirectory
        {
            get
            {
                var prop = FindPropertyByName("cleanDestinationDirectory");
                return Boolean.TryParse(prop, out bool result)
                    ? result
                    : throw new NullReferenceException();
            }
        }

        private string FindPropertyByName(string name)
            => this.Dto
                .Properties
                ?.Property
                ?.FirstOrDefault(prop => prop.Name == name)
                ?.Value;

        public override string ToString()
            => $"ArtifactDependency(buildConf={DependentBuildConfigurationId})";
    }
}