using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class VcsRootRepository : BaseRepository<VcsRootDto, VcsRootListDto>
    {
        public VcsRootDto Create(string xmlString)
        {
            using var strReader = new StringReader(xmlString);
            using var xmlReader = XmlReader.Create(strReader);
            var serializer = new XmlSerializer(typeof(NewVcsRoot));
            var newDto = serializer.Deserialize(xmlReader) as NewVcsRoot;

            // TODO: Refactor id checks elsewhere. TeamCity has a limited set of characters which are suitable
            // for Ids. - is not one of those characters.
            if (newDto.Id.Contains('-'))
            {
                throw new InvalidOperationException("Invalid character in id.");
            }

            var dto = newDto.ToVcsRootDto();
            _itemsById.Add(dto.Id, dto);
            return dto;
        }

        public VcsRootDto CreateVcsRestClientGit() => new VcsRootDto
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

        public VcsRootDto CreateVcs(Guid id) => new VcsRootDto
        {
            Id = $"Vcs_{id.ToString().ToLower().Replace('-', '_')}",
            Name = $"Vcs_{id.ToString().ToLower().Replace('-', '_')}"
        };
    }

    class VcsRootInstanceRepository : BaseRepository<VcsRootInstanceDto, VcsRootInstanceListDto>
    {
        public VcsRootInstanceDto CreateVcsInstance(VcsRootDto vcsRoot) => new VcsRootInstanceDto
        {
            Id = "3",
            VcsRootId = vcsRoot.Id,
            Name = "Bitbucket"
        };   
    }
}