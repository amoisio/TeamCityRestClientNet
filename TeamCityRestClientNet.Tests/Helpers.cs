using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Tests
{
    public static class TeamCityHelpers
    {
        public static async Task EmptyBuildQueue(TeamCity teamCity)
        {
            await foreach (var build in teamCity.BuildQueue.QueuedBuilds())
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

        public static async Task EnableAllAgents(TeamCity teamCity)
        {
            var agents = await teamCity.BuildAgents.All();
            foreach (var agent in agents)
            {
                await agent.Enable();
            }
        }

        public static async Task DisableAllAgents(TeamCity teamCity)
        {
            var agents = await teamCity.BuildAgents.All();
            foreach (var agent in agents)
            {
                await agent.Disable();
            }
        }
    }
}