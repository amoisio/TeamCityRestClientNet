using System;
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
        private readonly BuildRepository _builds;

        public BuildTypeRepository(BuildRepository builds)
        {
            this._builds = builds;
        }

        internal BuildTypeDto Create(string buildTypeXml)
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

        internal TagsDto Tags(string id)
        {
            var tags =_builds.All().Items
                .Where(build => build.Tags != null)
                .SelectMany(build => build.Tags.Tag);
            return new TagsDto
            {
                Tag = tags.ToList()
            };
        }

        internal TriggersDto Triggers(string id)
        {
            return new TriggersDto
            {
                Trigger = new List<TriggerDto>()
            };
        }

        internal ArtifactDependenciesDto ArtifactDependencies(string id)
        {
            return new ArtifactDependenciesDto
            {
                ArtifactDependency = new List<ArtifactDependencyDto>()
            };
        }

        internal void SetParameters(string id, string descriptor, string content)
        {
            // do nothing (BuildTypeDto does not expose parameters yet)
        }

        internal void SetSetting(string id, string descriptor, string content)
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
    }
}