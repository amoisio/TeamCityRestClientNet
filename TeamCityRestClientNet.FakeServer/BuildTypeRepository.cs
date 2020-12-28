using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class BuildTypeRepository 
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

        private static readonly List<BuildTypeDto> _buildTypes = new List<BuildTypeDto>
        {
            RestClient,
            TeamCityCli
        };

        public BuildTypeDto ById(string id) => _buildTypes.SingleOrDefault(u => u.Id == id);
        public BuildTypesDto All() => new BuildTypesDto { BuildType = _buildTypes };
    }
}