using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildTypeRepository : BaseRepository<BuildTypeDto, BuildTypeListDto>
    {
        public BuildTypeDto Create(string buildTypeXml)
        {
            using (var strReader = new StringReader(buildTypeXml))
            using (var xmlReader = XmlReader.Create(strReader))
            {
                var serializer = new XmlSerializer(typeof(NewBuildTypeDescription));
                var newDto = serializer.Deserialize(xmlReader) as NewBuildTypeDescription;
                var dto = new BuildTypeDto
                {
                    Id = newDto.Name,
                    Name = newDto.Name
                };
                this.Add(dto);
                return dto;
            }
        }

        public TagsDto Tags(BuildRepository builds, string id)
        {
            var tags = builds.All().Items
                .Where(build => build.Tags != null)
                .SelectMany(build => build.Tags.Tag);
            return new TagsDto
            {
                Tag = tags.ToList()
            };
        }

        public TriggersDto Triggers(string id)
        {
            return new TriggersDto
            {
                Trigger = new List<TriggerDto>()
            };
        }

        public ArtifactDependenciesDto ArtifactDependencies(string id)
        {
            return new ArtifactDependenciesDto
            {
                ArtifactDependency = new List<ArtifactDependencyDto>()
            };
        }

        public void SetParameters(string id, string descriptor, string content)
        {
            // do nothing (BuildTypeDto does not expose parameters yet)
        }

        public void SetSetting(string id, string descriptor, string content)
        {
            var buildType = ById(id);
            var setting = buildType.Settings.Property.FirstOrDefault(prop => prop.Name == descriptor);
            if (setting == null)
            {
                buildType.Settings.Property.Add(new NameValuePropertyDto
                {
                    Name = descriptor,
                    Value = content
                });
            }
            else
            {
                setting.Value = content;
            }
        }

        public BuildTypeDto CreateBuildTypeRestClient(ProjectDto project)
        {
            var type = new BuildTypeDto
            {
                Id = "TeamCityRestClientNet_RestClient",
                Name = "Rest Client",
                ProjectId = project.Id,
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
            project.BuildTypes.Items.Add(type);
            return type;
        }

        public BuildTypeDto CreateBuildTypeTeamCityCli(ProjectDto project)
        {
            var type = new BuildTypeDto
            {
                Id = "TeamCityCliNet_Cli",
                Name = "CLI",
                ProjectId = project.Id,
                Settings = new BuildTypeSettingsDto
                {
                    Property = new List<NameValuePropertyDto>
                {
                    new NameValuePropertyDto { Name = "buildNumberCounter", Value = "1" }
                }
                }
            };
            project.BuildTypes.Items.Add(type);
            return type;
        }
    }
}