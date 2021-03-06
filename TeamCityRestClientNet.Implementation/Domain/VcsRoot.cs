using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Domain
{
    class VcsRoot : Base<VcsRootDto>, IVcsRoot
    {
        private VcsRoot(VcsRootDto fullDto, TeamCityServer instance)
            : base(fullDto, instance)
        {
            this.Properties = this.Dto.Properties
                ?.Property
                ?.ToDictionary(prop => prop.Name, prop => prop.Value)
                ?? new Dictionary<string, string>();
        }

        public static async Task<IVcsRoot> Create(VcsRootDto dto, bool isFullDto, TeamCityServer instance)
        {
            var fullDto = isFullDto
                ? dto
                :await instance.Service.VcsRoot(dto.Id).ConfigureAwait(false);
            
            return new VcsRoot(fullDto, instance);
        }
        public Dictionary<string, string> Properties { get; }
        public string Url => GetNameValueProperty("url");
        public string DefaultBranch => GetNameValueProperty("branch");
        private string GetNameValueProperty(string name)
            => Properties.ContainsKey(name)
                ? Properties[name]
                : null;
        public override string ToString()
            => $"VcsRoot(id={Id}, name={Name}, url={Url})";
        public async Task Delete()
        {
            await Service.DeleteVcsRoot($"id:{Id}").ConfigureAwait(false);
        }
    }
}