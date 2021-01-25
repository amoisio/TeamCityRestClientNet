using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Domain
{
    class BuildProblem : IBuildProblem
    {
        private readonly BuildProblemDto _dto;

        public BuildProblem(BuildProblemDto dto)
        {
            _dto = dto;
        }

        public Id Id => new Id(_dto.Id);
        public BuildProblemType Type => new BuildProblemType(_dto.Type);
        public string Identity => _dto.Identity;
        public string Name => null;
        public string Href => null;
        public string WebUrl => null;

        public override string ToString()
            => $"BuildProblem(id={Id},type={Type},identity={Identity})";
    }
}