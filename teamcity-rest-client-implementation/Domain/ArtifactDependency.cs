using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class ArtifactDependency : Base<ArtifactDependencyDto>, IArtifactDependency
    {
        public ArtifactDependency(ArtifactDependencyDto dto, bool isFullDto, TeamCityInstance instance)
            : base(dto, isFullDto, instance)
        {

        }

        public IBuildConfiguration DependsOnBuildConfiguration
            => new BuildConfiguration(NotNullSync(dto => dto.SourceBuildType), false, Instance);

        public string Branch
            => FindPropertyByName("revisionBranch");

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
            => this.FullDtoSync.Properties
                ?.Property
                ?.FirstOrDefault(prop => prop.Name == name)
                ?.Value;

        public override string ToString()
            => $"ArtifactDependency(buildConf={DependsOnBuildConfiguration.Id.stringId})";

        protected override Task<ArtifactDependencyDto> FetchFullDto()
            => throw new NotSupportedException("Not supported, ArtifactDependency must be created with full dto.");
    }
}