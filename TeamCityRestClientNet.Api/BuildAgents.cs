using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public struct BuildAgentId
    {
        public BuildAgentId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface IBuildAgent
    {
        BuildAgentId Id { get; }
        string Name { get; }
        AsyncLazy<IBuildAgentPool> Pool { get; }
        bool Connected { get; }
        bool Enabled { get; }
        Task Enable();
        Task Disable();
        bool Authorized { get; }
        bool Outdated { get; }
        string IpAddress { get; }
        List<IParameter> Parameters { get; }
        IInfo EnabledInfo { get; }
        IInfo AuthorizedInfo { get; }
        AsyncLazy<IBuild> CurrentBuild { get; }
        string GetHomeUrl();
    }
    
    public interface IBuildAgentLocator
    {
        Task<IBuildAgent> Agent(BuildAgentId id);
        Task<IEnumerable<IBuildAgent>> All();
    }

    public interface IBuildAgentEnabledInfo : IInfo { }

    public interface IBuildAgentAuthorizedInfo : IInfo { }

    public struct BuildAgentPoolId
    {
        public BuildAgentPoolId(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;
    }

    public interface IBuildAgentPool
    {
        BuildAgentPoolId Id { get; }
        string Name { get; }
        AsyncLazy<List<IProject>> Projects { get; }
        AsyncLazy<List<IBuildAgent>> Agents { get; }
    }

    public interface IBuildAgentPoolLocator
    {
        Task<IBuildAgentPool> AgentPool(BuildAgentPoolId id);
        Task<IEnumerable<IBuildAgentPool>> All();
    }
}