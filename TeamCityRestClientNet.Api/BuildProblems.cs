using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public struct BuildProblemType
    {
        public BuildProblemType(string stringId)
        {
            this.stringId = stringId;
        }

        public readonly string stringId;
        public override string ToString() => this.stringId;

        public bool isSnapshotDependencyError
            => this.stringId == "SNAPSHOT_DEPENDENCY_ERROR_BUILD_PROCEEDS_TYPE"
            || this.stringId == "SNAPSHOT_DEPENDENCY_ERROR";

        public readonly static BuildProblemType FAILED_TESTS = new BuildProblemType("TC_FAILED_TESTS");
    }

    public interface IBuildProblem : IIdentifiable
    {
        BuildProblemType Type { get; }
        string Identity { get; }
    }

    public interface IBuildProblemOccurrence
    {
        IBuildProblem BuildProblem { get; }
        AsyncLazy<IBuild> Build { get; }
        string Details { get; }
        string AdditionalData { get; }
    }
}