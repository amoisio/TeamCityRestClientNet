using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class BuildAgentPoolRepository : BaseRepository<BuildAgentPoolDto, BuildAgentPoolListDto>
    {
        public BuildAgentPoolDto CreateDefaultPool() => new BuildAgentPoolDto
        {
            Id = "0",
            Name = "Default"
        };
    }
}