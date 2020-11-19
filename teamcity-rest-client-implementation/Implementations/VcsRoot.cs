using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Implementations
{
    class VcsRoot : Base<VcsRootDto>, IVcsRoot
    {
        public VcsRoot(VcsRootDto dto, bool isFullDto, TeamCityInstance instance)
            : base(dto, isFullDto, instance)
        {

        }

        public List<NameValueProperty> Properties
            => this.FullDto.Properties
                ?.Property
                ?.Select(prop => new NameValueProperty(prop))
                .ToList()
                ?? throw new NullReferenceException();


        public VcsRootId Id => new VcsRootId(IdString);

        public string Name
            => NotNull(dto => dto.Name);

        private string GetNameValueProperty(List<NameValueProperty> properties, string name)
            => properties.SingleOrDefault((prop) => prop.Name == name)?.Value;

        public string Url => GetNameValueProperty(this.Properties, "url");

        public string DefaultBranch 
            => GetNameValueProperty(this.Properties, "branch");

        public override string ToString()
            => $"VcsRoot(id={Id}, name={Name}, url={Url})";

        protected override async Task<VcsRootDto> FetchFullDto()
            => await Service.VcsRoot(IdString);
    }
}