using System;
using System.Collections.Generic;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Locators
{
    class TestRunsLocator : Locator, ITestRunsLocator
    {
        private int? limitResults;
        private int? pageSize;
        private BuildId? buildId;
        private TestId? testId;
        private ProjectId? affectedProjectId;
        private TestStatus? testStatus;
        private bool expandMultipleInvocations = false;

        public TestRunsLocator(TeamCityInstance instance) : base(instance) { }

        public IEnumerable<ITestRun> All()
        {
            throw new NotImplementedException();
            // override fun All(): Sequence<TestRun> {
            //     val statusLocator = When(testStatus) {
            //         null-> null
            //             TestStatus.FAILED-> "status:FAILURE"
            //             TestStatus.SUCCESSFUL-> "status:SUCCESS"
            //             TestStatus.IGNORED-> "ignored:true"
            //             TestStatus.UNKNOWN->error("Unsupported filter by test status UNKNOWN")
            //         }

            //     val count = SelectRestApiCountForPagedRequests(limitResults = limitResults, pageSize = pageSize)
            //         val parameters = ListOfNotNull(
            //                 count?.let { "count:$it" },
            //                 affectedProjectId?.let { "affectedProject:$it" },
            //                 buildId?.let { "build:$it" },
            //                 testId?.let { "test:$it" },
            //                 expandMultipleInvocations.let { "expandInvocations:$it" },
            //                 statusLocator
            //         )

            //         if (parameters.isEmpty())
            //     {
            //         throw IllegalArgumentException("At least one parameter should be specified")
            //         }

            //     val sequence = LazyPaging(instance, {
            //         val testOccurrencesLocator = parameters.joinToString(",")
            //             LOG.debug("Retrieving test occurrences from ${instance.serverUrl} using query '$testOccurrencesLocator'")

            //             return @lazyPaging instance.service.testOccurrences(locator = testOccurrencesLocator, fields = TestOccurrenceBean.filter)
            //         }) {
            //         testOccurrencesBean->
            //        Page(
            //                data = testOccurrencesBean.testOccurrence.map { TestRunImpl(it) },
            //                     nextHref = testOccurrencesBean.nextHref
            //             )
            //         }

            //     val limitResults1 = limitResults
            //         return if (limitResults1 != null) sequence.take(limitResults1) else sequence
            //     }
            // }

        }

        public ITestRunsLocator ExpandMultipleInvocations()
        {
            this.expandMultipleInvocations = true;
            return this;
        }

        public ITestRunsLocator ForBuild(BuildId buildId)
        {
            this.buildId = buildId;
            return this;
        }

        public ITestRunsLocator ForProject(ProjectId projectId)
        {
            this.affectedProjectId = projectId;
            return this;
        }

        public ITestRunsLocator ForTest(TestId testId)
        {
            this.testId = testId;
            return this;
        }

        public ITestRunsLocator LimitResults(int count)
        {
            this.limitResults = count;
            return this;
        }

        public ITestRunsLocator PageSize(int pageSize)
        {
            this.pageSize = pageSize;
            return this;
        }

        public ITestRunsLocator WithStatus(TestStatus testStatus)
        {
            this.testStatus = testStatus;
            return this;
        }
    }
}