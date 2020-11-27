using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Domain
{
    class TestRun : ITestRun
    {
        private readonly TestOccurrenceDto _dto;

        public TestRun(TestOccurrenceDto dto)
        {
            this._dto = dto;
        }

        public string Name => _dto.Name.SelfOrNullRef();

        public TestStatus Status => 
              _dto.Ignored == true ? TestStatus.IGNORED 
            : _dto.Status == "FAILURE" ? TestStatus.FAILED  
            : _dto.Status == "SUCCESS" ? TestStatus.SUCCESSFUL
            : TestStatus.UNKNOWN;

        public TimeSpan Duration => TimeSpan.FromMilliseconds(_dto.Duration ?? 0L);

        public string Details => Status switch 
        {
            TestStatus.IGNORED => _dto.IgnoreDetails,
            TestStatus.FAILED => _dto.Details,
            _ => String.Empty
        };
        public bool Ignored => _dto.Ignored ?? false;
        public bool CurrentlyMuted => _dto.CurrentlyMuted ?? false;
        public bool Muted => _dto.Muted ?? false;
        public bool NewFailure => _dto.NewFailure ?? false;
        public BuildId BuildId => new BuildId(_dto.Build.SelfOrNullRef().Id.SelfOrNullRef());
        public BuildId? FixedIn => 
            (_dto.NextFixed?.Id == null)
                ? default(BuildId?)
                : new BuildId(_dto.NextFixed.SelfOrNullRef().Id.SelfOrNullRef());
        public BuildId? FirstFailedIn =>
            (_dto.FirstFailed?.Id == null)
                ? default(BuildId?)
                : new BuildId(_dto.FirstFailed.SelfOrNullRef().Id.SelfOrNullRef());

        public TestId TestId => new TestId(_dto.Test.SelfOrNullRef().Id.SelfOrNullRef());

        public override string ToString() =>
            $"Test(name={Name}, status={Status}, duration={Duration}, details={Details})";
    }
}