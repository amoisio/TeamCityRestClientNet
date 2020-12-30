using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class VcsRootRepository : BaseRepository<VcsRootDto>
    {
        public static readonly VcsRootDto RestClientGit = new VcsRootDto
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

        public static readonly VcsRootDto Vcs1 = new VcsRootDto
        {
            Id = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269",
            Name = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269"
        };

        public static readonly VcsRootDto Vcs2 = new VcsRootDto
        {
            Id = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6",
            Name = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6"
        };

        public static readonly VcsRootDto Vcs3 = new VcsRootDto
        {
            Id = "Vcs_ExtraOne",
            Name = "Vcs_ExtraOne"
        };

        public static readonly VcsRootDto Vcs4 = new VcsRootDto
        {
            Id = "Vcs_AnotherOne",
            Name = "Vcs_AnotherOne"
        };

        public VcsRootRepository() 
            : base(root => root.Id, RestClientGit, Vcs1, Vcs2, Vcs3, Vcs4) { }

        public VcsRootListDto All() => new VcsRootListDto { VcsRoot = AllItems() };

        public VcsRootDto Create(string xmlString)
        {
            using(var strReader = new StringReader(xmlString))
            using (var xmlReader = XmlReader.Create(strReader))
            {
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
        }

        public VcsRootDto Delete(string id)
        {
            var root = ById(id);
            if (root != null)
            {
                _itemsById.Remove(id);
                return root;
            }
            else
                throw new ArgumentException($"Vcs root with id {id} not found.");
        }
    }
}