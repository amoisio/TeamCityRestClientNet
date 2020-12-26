using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Domain
{
    class BuildProblemOccurrence : IBuildProblemOccurrence
    {
        private readonly BuildProblemOccurrenceDto _dto;
        private readonly TeamCityServer _instance;

        public BuildProblemOccurrence(BuildProblemOccurrenceDto dto, TeamCityServer instance)
        {
            this.Build = new AsyncLazy<IBuild>(async () 
                => await Domain.Build.Create(dto.Build.Id, instance).ConfigureAwait(false));
            this._dto = dto;
            this._instance = instance;
        }

        public IBuildProblem BuildProblem => new BuildProblem(_dto.Problem);
        public AsyncLazy<IBuild> Build { get; }
        public string Details => _dto.Details ?? string.Empty;
        public string AdditionalData => _dto.AdditionalData;

        public override string ToString()
            => $"BuildProblemOccurrence(build={_dto.Build.Id},problem={BuildProblem},details={Details},additionalData={AdditionalData})";
    }
}