using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nito.AsyncEx;

using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Domain 
{
    class BuildAgentPool : Base<BuildAgentPoolDto>, IBuildAgentPool
    {
        private BuildAgentPool(BuildAgentPoolDto fullDto, TeamCityServer instance)
            : base(fullDto, instance)
        {
            this.Projects = new AsyncLazy<List<IProject>>(async ()
                => {
                    var tasks = this.Dto.Projects
                        ?.Project
                        ?.Select(project => Project.Create(project, false, Instance));
                    
                    return tasks != null
                        ? (await Task.WhenAll(tasks).ConfigureAwait(false)).ToList()
                        : new List<IProject>();
                });

            this.Agents = new AsyncLazy<List<IBuildAgent>>(async ()
                => {
                    var tasks = this.Dto.Agents
                        ?.Agent
                        ?.Select(agent => BuildAgent.Create(agent.Id, Instance));
                    
                    return tasks != null
                        ? (await Task.WhenAll(tasks).ConfigureAwait(false)).ToList()
                        : new List<IBuildAgent>();
                });
        }

        public static async Task<BuildAgentPool> Create(string idString, TeamCityServer instance)
        {
            var dto = await instance.Service.AgentPool($"id:{idString}").ConfigureAwait(false);
            return new BuildAgentPool(dto, instance);
        }

        public BuildAgentPoolId Id => new BuildAgentPoolId(IdString);
        public string Name => NotNull(dto => dto.Name);
       public AsyncLazy<List<IProject>> Projects { get; }
        public AsyncLazy<List<IBuildAgent>> Agents { get; }

        public override string ToString()
            => $"BuildAgentPool(id={Id}, name={Name})";
    }
}