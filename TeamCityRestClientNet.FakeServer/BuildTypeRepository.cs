using System.Collections.Generic;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildTypeRepository : BaseRepository<BuildTypeDto>
    {
        public static readonly BuildTypeDto RestClient = new BuildTypeDto
        {
            Id = "TeamCityRestClientNet_RestClient",
            Name = "Rest Client",
            ProjectId = "TeamCityRestClientNet",
            Settings = new BuildTypeSettingsDto
            {
                Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "artifactRules", Value = "artifacts" },
                    new NameValuePropertyDto { Name = "buildNumberCounter", Value = "138" },
                    new NameValuePropertyDto { Name = "cleanBuild", Value = "true" },
                    new NameValuePropertyDto { Name = "publishArtifactCondition", Value = "SUCCESSFUL" }
                }
            }
        };

        public static readonly BuildTypeDto TeamCityCli = new BuildTypeDto
        {
            Id = "TeamCityCliNet_Cli",
            Name = "CLI",
            ProjectId = "TeamCityCliNet",
            Settings = new BuildTypeSettingsDto
            {
                Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "buildNumberCounter", Value = "1" }
                }
            }
        };

        static BuildTypeRepository() { }

        public BuildTypeRepository() : base(type => type.Id, TeamCityCli, RestClient) { }

        public BuildTypesDto All() => new BuildTypesDto { BuildType = AllItems() };
    }
}