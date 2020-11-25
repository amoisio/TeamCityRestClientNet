using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeamCityRestClientNet.Service
{
    class TeamCityService : ITeamCityService
    {
        private readonly HttpClient _client;

        public TeamCityService(HttpClient client)
        {
            this._client = client;
        }

        public string ServerUrlBase => throw new System.NotImplementedException();

        public Task AddTag(string buildId, string tag)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildAgentPoolsDto> AgentPools()
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildAgentPoolDto> AgentPools(string agentLocator = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildAgentsDto> Agents()
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildAgentDto> Agents(string agentLocator = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<ArtifactFileListDto> ArtifactChildren(string buildId, string artifactPath, string locator, string fields)
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> ArtifactContent(string buildId, string artifactPath)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildDto> Build(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildTypeDto> BuildConfiguration(string buildTypeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> BuildLog(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildListDto> Builds(string buildLocator)
        {
            throw new System.NotImplementedException();
        }

        public Task<ArtifactDependenciesDto> BuildTypeArtifactDependencies(string buildTypeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<TagsDto> BuildTypeTags(string buildTypeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<TriggersDto> BuildTypeTriggers(string buildTypeId)
        {
            throw new System.NotImplementedException();
        }

        public Task CancelBuild(string buildId, BuildCancelRequestDto value)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChangeDto> Change(string buildType, string version)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChangeDto> Change(string changeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildListDto> ChangeFirstBuilds(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChangesDto> Changes(string locator, string fields)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildTypeDto> CreateBuildType(string buildTypeXml)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProjectDto> CreateProject(string projectDescriptionXml)
        {
            throw new System.NotImplementedException();
        }

        public Task<VcsRootDto> CreateVcsRoot(string vcsRootXml)
        {
            throw new System.NotImplementedException();
        }

        public Task<InvestigationDto> Investigation(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<InvestigationListDto> Investigations(string investigationLocator)
        {
            throw new System.NotImplementedException();
        }

        public Task Pin(string buildId, string comment)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildProblemOccurrencesDto> ProblemOccurrences(string locator, string fields)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProjectDto> Project(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildListDto> QueuedBuilds(string locator)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveQueuedBuild(string buildId, BuildCancelRequestDto value)
        {
            throw new System.NotImplementedException();
        }

        public Task ReplaceTags(string buildId, TagsDto tags)
        {
            throw new System.NotImplementedException();
        }

        public Task<ParametersDto> ResultingProperties(string buildId)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Root<T>(string path)
        {
            throw new System.NotImplementedException();
        }

        public Task SetBuildTypeParameter(string buildTypeId, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task SetBuildTypeSettings(string buildTypeId, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task SetComment(string buildId, string comment)
        {
            throw new System.NotImplementedException();
        }

        public Task SetProjectParameter(string projectId, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task<TestOccurrencesDto> TestOccurrences(string locator, string fields)
        {
            throw new System.NotImplementedException();
        }

        public Task<TriggeredBuildDto> TriggerBuild(TriggerBuildRequestDto value)
        {
            throw new System.NotImplementedException();
        }

        public Task Unpin(string buildId, string comment)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserListDto> Users()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserDto> Users(string userLocator)
        {
            throw new System.NotImplementedException();
        }

        public Task<VcsRootDto> VcsRoot(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<VcsRootListDto> VcsRoots(string locator = null)
        {
            throw new System.NotImplementedException();
        }
    }
}