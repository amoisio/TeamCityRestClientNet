using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Domain 
{
    class BuildRunningInfo : IBuildRunningInfo
    {
        private readonly BuildRunningInfoDto _dto;

        public BuildRunningInfo(BuildRunningInfoDto dto)
        {
            this._dto = dto;
        }

        public int PercentageComplete => this._dto.PercentageComplete;
        public long ElapsedSeconds => this._dto.ElapsedSeconds;
        public long EstimatedTotalSeconds => this._dto.EstimatedTotalSeconds;
        public bool Outdated => this._dto.Outdated;
        public bool ProbablyHanging => this._dto.ProbablyHanging;
    }
}