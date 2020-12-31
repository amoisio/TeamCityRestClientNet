using System.Collections.Generic;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class DataBuilder
    {
        private readonly UserDto UserJohnDoe = new UserDto
        {
            Id = "1",
            Name = "John Doe",
            Username = "jodoe",
            Email = "john.doe@mailinator.com"
        };

        private readonly UserDto UserJaneDoe = new UserDto
        {
            Id = "2",
            Name = "Jane Doe",
            Username = "jadoe",
            Email = "jane.doe@mailinator.com"
        };

        private readonly UserDto UserDunkinDonuts = new UserDto
        {
            Id = "3",
            Name = "Dunkin' Donuts",
            Username = "dunkin",
            Email = "dunkin@mailinator.com"
        };

        private readonly UserDto UserMacCheese = new UserDto
        {
            Id = "4",
            Name = "Mac Cheese",
            Username = "maccheese",
            Email = "maccheese@mailinator.com"
        };

        private readonly VcsRootDto VcsRestClientGit = new VcsRootDto
        {
            Id = "TeamCityRestClientNet_Bitbucket",
            Name = "Bitbucket",
            Properties = new NameValuePropertiesDto
            {
                Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "agentCleanFilesPolicy", Value = "ALL_UNTRACKED" },
                    new NameValuePropertyDto { Name = "agentCleanPolicy", Value = "ON_BRANCH_CHANGE" },
                    new NameValuePropertyDto { Name = "authMethod", Value = "PASSWORD" },
                    new NameValuePropertyDto { Name = "branch", Value = "refs/heads/master" },
                    new NameValuePropertyDto { Name = "ignoreKnownHosts", Value = "true" },
                    new NameValuePropertyDto { Name = "secure:password" },
                    new NameValuePropertyDto { Name = "submoduleCheckout", Value = "CHECKOUT" },
                    new NameValuePropertyDto { Name = "teamcity:branchSpec", Value="+:*" },
                    new NameValuePropertyDto { Name = "url", Value = "https://noexist@bitbucket.org/joedoe/teamcityrestclientnet.git" },
                    new NameValuePropertyDto { Name = "useAlternates", Value = "true" },
                    new NameValuePropertyDto { Name = "usernameStyle", Value = "USERID" }
                }
            }
        };

        private readonly VcsRootDto Vcs1 = new VcsRootDto
        {
            Id = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269",
            Name = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269"
        };

        private readonly VcsRootDto Vcs2 = new VcsRootDto
        {
            Id = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6",
            Name = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6"
        };

        private readonly VcsRootDto Vcs3 = new VcsRootDto
        {
            Id = "Vcs_ExtraOne",
            Name = "Vcs_ExtraOne"
        };

        private readonly VcsRootDto Vcs4 = new VcsRootDto
        {
            Id = "Vcs_AnotherOne",
            Name = "Vcs_AnotherOne"
        };

        private readonly BuildTypeDto BuildTypeRestClient = new BuildTypeDto
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

        private readonly BuildTypeDto BuildTypeTeamCityCli = new BuildTypeDto
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

        private readonly ProjectDto RootProject = new ProjectDto
        {
            Id = "_Root",
            Name = "<Root project>"
        };

        private readonly ProjectDto RestClientProject = new ProjectDto
        {
            Id = "TeamCityRestClientNet",
            ParentProjectId = "_Root",
            Name = "TeamCity Rest Client .NET",
            Parameters = new ParametersDto
            {
                Property = new List<ParameterDto>
                {
                    new ParameterDto("configuration_parameter", "6692e7bf-c9a4-4941-9e89-5dde9417f05f"),
                }
            }
        };

        private readonly ProjectDto TeamCityCliProject = new ProjectDto
        {
            Id = "TeamCityCliNet",
            ParentProjectId = "_Root",
            Name = "TeamCity CLI .NET"
        };

        private readonly ProjectDto Project1 = new ProjectDto
        {
            Id = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4",
            ParentProjectId = "_Root",
            Name = "Project_e8fbb7af_1267_4df8_865f_7be55fdd54c4"
        };

        public DataBuilder()
        {
            Users = new UserRepository();
            VcsRoots = new VcsRootRepository();
            BuildTypes = new BuildTypeRepository();
            Projects = new ProjectRepository();
        }
        public void Load()
        {
            BuildTypes.Add(BuildTypeRestClient);
            BuildTypes.Add(BuildTypeTeamCityCli);

            RootProject.Projects.Project.Add(RestClientProject);
            RootProject.Projects.Project.Add(TeamCityCliProject);
            RootProject.Projects.Project.Add(Project1);
            Projects.Add(RootProject);
            RestClientProject.BuildTypes.BuildType.Add(BuildTypeRestClient);
            Projects.Add(RestClientProject);
            TeamCityCliProject.BuildTypes.BuildType.Add(BuildTypeTeamCityCli);
            Projects.Add(TeamCityCliProject);
            Projects.Add(Project1);

            Users.Add(UserJohnDoe);
            Users.Add(UserJaneDoe);
            Users.Add(UserDunkinDonuts);
            Users.Add(UserMacCheese);

            VcsRoots.Add(VcsRestClientGit);
            VcsRoots.Add(Vcs1);
            VcsRoots.Add(Vcs2);
            VcsRoots.Add(Vcs3);
            VcsRoots.Add(Vcs4);
        }

        public UserRepository Users { get; private set; }
        public VcsRootRepository VcsRoots { get; private set; }
        public BuildTypeRepository BuildTypes { get; private set; }
        public ProjectRepository Projects { get; private set; }

    }
}