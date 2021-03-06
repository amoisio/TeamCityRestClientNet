using System;
using System.Collections.Generic;

namespace TeamCityRestClientNet.Api
{
    public interface ITestRun
    {
        string Name { get; }
        TestStatus Status { get; }

        /**
         * Test run duration. It may be ZERO if a test finished too fast (<1ms)
         */
        TimeSpan Duration { get; }
        string Details { get; }
        bool Ignored { get; }
        /**
         * Current 'muted' status of this test on TeamCity
         */
        bool CurrentlyMuted { get; }
        /**
         * Muted at the moment of running tests
         */
        bool Muted { get; }
        /**
         * Newly failed test or not
         */
        bool NewFailure { get; }
        Id BuildId { get; }
        Id? FixedIn { get; }
        Id? FirstFailedIn { get; }
        Id TestId { get; }
    }
    
    public interface ITestRunsLocator
    {
        ITestRunsLocator LimitResults(int count);
        ITestRunsLocator PageSize(int pageSize);
        ITestRunsLocator ForBuild(Id buildId);
        ITestRunsLocator ForTest(Id testId);
        ITestRunsLocator ForProject(Id projectId);
        ITestRunsLocator WithStatus(TestStatus testStatus);
        /**
         * If expandMultipleInvocations is enabled, individual runs of tests, which were executed several
         * times in same build, are returned as separate entries.
         * By default such runs are aggregated into a single value, duration property will be the sum of durations
         * of individual runs, and status will be SUCCESSFUL if and only if all runs are successful.
         */
        ITestRunsLocator ExpandMultipleInvocations();
        IAsyncEnumerable<ITestRun> All();
    }
    
    public enum TestStatus
    {
        SUCCESSFUL,
        IGNORED,
        FAILED,
        UNKNOWN
    }
}