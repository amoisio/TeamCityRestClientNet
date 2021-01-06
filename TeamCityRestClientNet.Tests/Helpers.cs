using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Tests
{
    public static class TeamCityHelpers
    {
        public static async Task EmptyBuildQueue(TeamCity teamCity)
        {
            await foreach (var build in teamCity.BuildQueue.All())
            {
                await teamCity.BuildQueue.RemoveBuild(build.Id);
            }
        }

        public static async Task CancelAllRunningBuilds(TeamCity teamCity)
        {
            var runningBuilds = teamCity.Builds
                .WithAllBranches()
                .OnlyRunning()
                .All();

            await foreach (var build in runningBuilds)
            {
                await build.Cancel();
            }
        }
    }
}