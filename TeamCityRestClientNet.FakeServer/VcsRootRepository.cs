using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class VcsRootRepository
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

        private static readonly Dictionary<string, VcsRootDto> _roots = new Dictionary<string, VcsRootDto>
        {
            { 
                RestClientGit.Id, 
                RestClientGit 
            },
            { "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269", new VcsRootDto
                {
                    Id = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269",
                    Name = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269"
                }
            },
            { "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6", new VcsRootDto
                {
                    Id = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6",
                    Name = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6"
                }
            }
        };

        public VcsRootDto ById(string id) => _roots.ContainsKey(id) ? _roots[id] : default(VcsRootDto);

        public VcsRootListDto All() => new VcsRootListDto { VcsRoot = _roots.Values.ToList() };

        public VcsRootDto Create(string xmlString)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "vcs-root";
            var overrides = new XmlAttributeOverrides();
            overrides.Add(typeof(VcsRootDto), "Name", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute() });
            overrides.Add(typeof(IdDto), "Id", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute() });
            overrides.Add(typeof(VcsRootDto), "VcsName", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute() });
            overrides.Add(typeof(ProjectDto), "Id", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute() });
            overrides.Add(typeof(NameValuePropertyDto), "Name", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute() });
            overrides.Add(typeof(NameValuePropertyDto), "Value", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute() });

            var serializer = new XmlSerializer(typeof(VcsRootDto), overrides, null, xRoot, null);
            using (var strReader = new StringReader(xmlString))
            using (var xmlReader = new ClientXmlReader(XmlReader.Create(strReader)))
            {
                var dto = serializer.Deserialize(xmlReader) as VcsRootDto;

                // TODO: Refactor id checks elsewhere. TeamCity has a limited set of characters which are suitable
                // for Ids. - is not one of those characters.
                if (dto.Id.Contains('-'))
                {
                    throw new InvalidOperationException("Invalid character in id.");
                }

                _roots.Add(dto.Id, dto);
                return dto;
            }
        }

        public VcsRootDto Delete(string id)
        {
            var root = ById(id);
            if (root != null)
            {
                _roots.Remove(id);
                return root;
            }
            else
                throw new ArgumentException($"Vcs root with id {id} not found.");
        }
    }
}