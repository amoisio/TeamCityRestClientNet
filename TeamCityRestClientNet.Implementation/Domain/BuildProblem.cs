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

        public BuildProblemId Id => new BuildProblemId(_dto.Id);
        public BuildProblemType Type => new BuildProblemType(_dto.Type);
        public string Identity => _dto.Identity;

        public override string ToString()
            => $"BuildProblem(id={Id.stringId},type={Type},identity={Identity})";
    }
}