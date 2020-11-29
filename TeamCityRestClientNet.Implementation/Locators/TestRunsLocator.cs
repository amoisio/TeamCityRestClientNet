using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Locators
{
    class TestRunsLocator : Locator, ITestRunsLocator
    {
        private int? _limitResults;
        private int? _pageSize;
        private BuildId? _buildId;
        private TestId? _testId;
        private ProjectId? _affectedProjectId;
        private TestStatus? _testStatus;
        private bool _expandMultipleInvocations = false;

        public TestRunsLocator(TeamCityServer instance) : base(instance) { }

        public IAsyncEnumerable<ITestRun> All()
        {
            var statusLocator = _testStatus switch
            {
                TestStatus.FAILED => "status:FAILURE",
                TestStatus.SUCCESSFUL => "status:SUCCESS",
                TestStatus.IGNORED => "ignored:true",
                _ => throw new Exception($"Unsupported filter by test status {_testStatus}")
            };
            var count = Utilities.SelectRestApiCountForPagedRequests(_limitResults, _pageSize);
            var parameters = Utilities.ListOfNotNull(
                count?.Let(val => $"count:{val}"),
                _affectedProjectId?.Let(val => $"affectedProject:{val}"),
                _buildId?.Let(val => $"build:{val}"),
                _testId?.Let(val => $"test:{val}"),
                _expandMultipleInvocations.Let(val => $"expandInvocations:{val}"),
                statusLocator
            );

            if (parameters.IsEmpty())
            {
                throw new ArgumentException("At least one parameter should be specified");

            }

            var sequence = new Paged<ITestRun, TestOccurrencesDto>(
                Instance,
                async () =>
                {
                    var testOccurrencesLocator = String.Join(",", parameters);
                    // LOG.debug("Retrieving test occurrences from ${instance.serverUrl} using query '$testOccurrencesLocator'")
                    return await Service
                        .TestOccurrences(testOccurrencesLocator, TestOccurrenceDto.FILTER)
                        .ConfigureAwait(false);
                },
                async (list) =>
                {
                    return await Task.Run(() =>
                    {
                        var runs = list.TestOccurrence.Select(test => new TestRun(test)).ToArray();
                        return new Page<ITestRun>(
                            runs,
                            list.NextHref
                        );
                    }).ConfigureAwait(false);
                }
            );
            return _limitResults.HasValue
                ? sequence.Take(_limitResults.Value)
                : sequence;
        }

        public ITestRunsLocator ExpandMultipleInvocations()
        {
            this._expandMultipleInvocations = true;
            return this;
        }

        public ITestRunsLocator ForBuild(BuildId buildId)
        {
            this._buildId = buildId;
            return this;
        }

        public ITestRunsLocator ForProject(ProjectId projectId)
        {
            this._affectedProjectId = projectId;
            return this;
        }

        public ITestRunsLocator ForTest(TestId testId)
        {
            this._testId = testId;
            return this;
        }

        public ITestRunsLocator LimitResults(int count)
        {
            this._limitResults = count;
            return this;
        }

        public ITestRunsLocator PageSize(int pageSize)
        {
            this._pageSize = pageSize;
            return this;
        }

        public ITestRunsLocator WithStatus(TestStatus testStatus)
        {
            this._testStatus = testStatus;
            return this;
        }
    }
}