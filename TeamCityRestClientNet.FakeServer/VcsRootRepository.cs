using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    public class VcsRootRepository
    {
        private readonly List<VcsRootDto> _roots = new List<VcsRootDto>
        {
            new VcsRootDto {
                Id = "TeamCityRestClientNet_Bitbucket",
                Name = "Bitbucket",
                Properties = new NameValuePropertiesDto {
                    Property = new List<NameValuePropertyDto> {
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
            },
            new VcsRootDto {
                Id = "af57aa45_ddd0_4e39_8163_b685be56e269",
                Name = "Vcs_af57aa45_ddd0_4e39_8163_b685be56e269" },
            new VcsRootDto {
                Id = "b283d84e_6dc1_4fa8_87cf_1fecf65aada6",
                Name = "Vcs_b283d84e_6dc1_4fa8_87cf_1fecf65aada6" }
        };
        
        public VcsRootDto ById(string id) => _roots.SingleOrDefault(u => u.Id == id);
        public VcsRootListDto All() => new VcsRootListDto { VcsRoot = _roots };
    }
}