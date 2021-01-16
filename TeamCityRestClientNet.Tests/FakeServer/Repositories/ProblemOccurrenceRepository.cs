using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class ProblemOccurrenceRepository : BaseRepository<BuildProblemOccurrenceDto,  BuildProblemOccurrenceListDto>
    {
    }
}