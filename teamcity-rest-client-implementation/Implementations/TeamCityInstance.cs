using System;
using System.Collections.Generic;
using System.Net;
using BAMCIS.Util.Concurrent;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Implementations
{
    class TeamCityInstance : TeamCityInstanceBase
    {
        public const string TEAMCITY_DATETIME_FORMAT = "yyyyMMdd'T'HHmmssZ";
        public const string TEAMCITY_DEFAUL_LOCALE = "en-US";
        private readonly string _serverUrl;
        private readonly string _serverUrlBase;
        private readonly string _authHeader;
        private readonly bool _logResponses;
        private readonly TimeUnit _unit;
        private readonly long _timeout;

        internal TeamCityInstance(
            string serverUrl,
            string serverUrlBase,
            string authHeader,
            bool logResponses,
            TimeUnit unit,
            long timeout = 2)
        {

            //     private val restLog = LoggerFactory.getLogger(LOG.name + ".rest")
            this._serverUrl = serverUrl;
            this._serverUrlBase = serverUrlBase;
            this._authHeader = authHeader;
            this._logResponses = logResponses;
            this._unit = unit;
            this._timeout = timeout;
        }

        protected override string ServerUrl => throw new System.NotImplementedException();

        public override IBuild Build(BuildId id)
        {
            throw new System.NotImplementedException();
        }

        public override IBuild Build(BuildConfigurationId buildConfigurationId, string number)
        {
            throw new System.NotImplementedException();
        }

        public override IBuildAgentPoolLocator BuildAgentPools()
        {
            throw new System.NotImplementedException();
        }

        public override IBuildAgentLocator BuildAgents()
        {
            throw new System.NotImplementedException();
        }

        public override IBuildConfiguration BuildConfiguration(BuildConfigurationId id)
        {
            throw new System.NotImplementedException();
        }

        public override IBuildQueue BuildQueue()
        {
            throw new System.NotImplementedException();
        }

        public override IBuildLocator Builds()
        {
            throw new System.NotImplementedException();
        }

        public override IChange Change(BuildConfigurationId buildConfigurationId, string vcsRevision)
        {
            throw new System.NotImplementedException();
        }

        public override IChange Change(ChangeId id)
        {
            throw new System.NotImplementedException();
        }

        public override IInvestigationLocator Investigations()
        {
            throw new System.NotImplementedException();
        }

        public override IProject Project(ProjectId id)
        {
            throw new System.NotImplementedException();
        }

        public override IProject RootProject()
        {
            throw new System.NotImplementedException();
        }

        public override ITestRunsLocator TestRuns()
        {
            throw new System.NotImplementedException();
        }

        public override IUser User(UserId id)
        {
            throw new System.NotImplementedException();
        }

        public override IUser User(string userName)
        {
            throw new System.NotImplementedException();
        }

        public override IUserLocator Users()
        {
            throw new System.NotImplementedException();
        }

        public override IVcsRoot VcsRoot(VcsRootId id)
        {
            throw new System.NotImplementedException();
        }

        public override IVcsRootLocator VcsRoots()
        {
            throw new System.NotImplementedException();
        }

        public override TeamCityInstanceBase WithLogResponses()
            => new TeamCityInstance(_serverUrl, _serverUrlBase, _authHeader, true, TimeUnit.MINUTES);

        public override TeamCityInstanceBase WithTimeout(long timeout, TimeUnit unit)
            => new TeamCityInstance(_serverUrl, _serverUrlBase, _authHeader, true, unit, timeout);


        public ITeamCityService Service => throw new NotImplementedException();

        //     override fun Investigations(): InvestigationLocator = InvestigationLocatorImpl(this)

        //     override fun Build(id: BuildId): Build = BuildImpl(
        //             BuildBean().also { it.id = id.stringId }, false, this)

        //     override fun Build(buildConfigurationId: BuildConfigurationId, number: String): Build ? =
        //             BuildLocatorImpl(this).fromConfiguration(buildConfigurationId).withNumber(number).latest()

        //     override fun BuildConfiguration(id: BuildConfigurationId): BuildConfiguration =
        //             BuildConfigurationImpl(BuildTypeBean().also { it.id = id.stringId }, false, this)

        //     override fun VcsRoots(): VcsRootLocator = VcsRootLocatorImpl(this)

        //     override fun VcsRoot(id: VcsRootId): VcsRoot = VcsRootImpl(service.vcsRoot(id.stringId), true, this)

        //     override fun Project(id: ProjectId): Project = ProjectImpl(ProjectBean().let { it.id = id.stringId; it }, false, this)

        //     override fun RootProject(): Project = Rroject(ProjectId("_Root"))

        //     override fun User(id: UserId): User =
        //             UserImpl(UserBean().also { it.id = id.stringId }, false, this)

        //     override fun User(userName: String): User {
        //         val bean = service.users("username:$userName")
        //         return UserImpl(bean, true, this)
        //     }

        //     override fun Users(): UserLocator = UserLocatorImpl(this)

        //     override fun Change(buildConfigurationId: BuildConfigurationId, vcsRevision: String): Change =
        //             ChangeImpl(service.change(
        //                     buildType = buildConfigurationId.stringId, version = vcsRevision), true, this)

        //     override fun Change(id: ChangeId): Change =
        //             ChangeImpl(ChangeBean().also { it.id = id.stringId }, false, this)

        //     override fun BuildQueue(): BuildQueue = BuildQueueImpl(this)

        //     override fun BuildAgents(): BuildAgentLocator = BuildAgentLocatorImpl(this)

        //     override fun BuildAgentPools(): BuildAgentPoolLocator = BuildAgentPoolLocatorImpl(this)

        //     override fun GetWebUrl(projectId: ProjectId, branch: String ?): String =
        //              Project(projectId).getHomeUrl(branch = branch)

        //     override fun GetWebUrl(buildConfigurationId: BuildConfigurationId, branch: String ?): String =
        //              BuildConfiguration(buildConfigurationId).getHomeUrl(branch = branch)

        //     override fun GetWebUrl(buildId: BuildId): String =
        //             Build(buildId).getHomeUrl()

        //     override fun GetWebUrl(changeId: ChangeId, specificBuildConfigurationId: BuildConfigurationId ?, includePersonalBuilds: Boolean ?): String =
        //               Change(changeId).getHomeUrl(
        //                       specificBuildConfigurationId = specificBuildConfigurationId,
        //                       includePersonalBuilds = includePersonalBuilds
        //               )

        //     override fun QueuedBuilds(projectId: ProjectId ?): List < Build > =
        //              BuildQueue().queuedBuilds(projectId = projectId).toList()

        //     override fun TestRuns(): TestRunsLocator = TestRunsLocatorImpl(this)


        //     private var client = OkHttpClient.Builder()
        //             .readTimeout(timeout, unit)
        //             .writeTimeout(timeout, unit)
        //             .connectTimeout(timeout, unit)
        //             .addInterceptor(RetryInterceptor())
        //             .dispatcher(Dispatcher(
        //                     //by default non-daemon threads are used, and it blocks JVM from exit
        //                     ThreadPoolExecutor(0, Int.MAX_VALUE, 60, TimeUnit.SECONDS,
        //                             SynchronousQueue(),
        //                             object: ThreadFactory {
        //                                 private val count = AtomicInteger(0)
        //                                 override fun NewThread(r: Runnable) = Nhread(
        //                                         block = { r.run() },
        //                                         isDaemon = true,
        //                                         start = false,
        //                                         name = "TeamCity-Rest-Client - OkHttp Dispatcher - ${count.incrementAndGet()}"
        //                                 )
        //                             }
        //             )))
        //             .build()

        //     internal val service = RestAdapter.Builder()
        //             .setClient(Ok3Client(client))
        //             .setEndpoint("$serverUrl$serverUrlBase")
        //             .setLog { restLog.debug(if (authHeader != null) it.replace(authHeader, "[REDACTED]") else it) }
        //             .setLogLevel(if (logResponses) RestAdapter.LogLevel.FULL else RestAdapter.LogLevel.HEADERS_AND_ARGS)
        //             .setRequestInterceptor
        // {
        //     request->
        //                 if (authHeader != null)
        //     {
        //         request.addHeader("Authorization", authHeader)
        //                 }
        // }
        //             .setErrorHandler
        // {
        //     retrofitError->
        // val responseText = try
        //     {
        //         retrofitError.response.body.`in`().reader().use { it.readText() }
        //     }
        //     catch (t: Throwable) {
        //         LOG.warn("Exception while reading error response text: ${t.message}", t)
        //                     ""
        //                 }

        //     throw TeamCityConversationException("Failed to connect to ${retrofitError.url}: ${retrofitError.message} $responseText", retrofitError)
        //             }
        //             .build()
        //             .create(TeamCityService::class.java)

        //     override fun Close()
        // {
        //     fun CatchAll(action: ()->Unit): Unit = try
        //     {
        //         Action()
        //         }
        //     catch (t: Throwable) {
        //         LOG.warn("Failed to close connection. ${t.message}", t)
        //         }

        //     catchAll { client.dispatcher.cancelAll() }
        //     catchAll { client.dispatcher.executorService.shutdown() }
        //     catchAll { client.connectionPool.evictAll() }
        //     catchAll { client.cache?.close() }
        //     }


        // private fun String.urlencode(): String = URLEncoder.encode(this, "UTF-8")

        // private fun GetUserUrlPage(serverUrl: String,
        //                            pageName: String,
        //                            tab: String ? = null,
        //                            projectId: ProjectId ? = null,
        //                            buildId: BuildId ? = null,
        //                            testNameId: TestId ? = null,
        //                            userId: UserId ? = null,
        //                            modId: ChangeId ? = null,
        //                            personal: Boolean ? = null,
        //                            buildTypeId: BuildConfigurationId ? = null,
        //                            branch: String ? = null): String
        // {
        //     val params = mutableListOf<String>()

        //     tab?.let { params.add("tab=" + tab.urlencode()) }
        //     projectId?.let { params.add("projectId=" + projectId.stringId.urlencode()) }
        //     buildId?.let { params.add("buildId=" + buildId.stringId.urlencode()) }
        //     testNameId?.let { params.add("testNameId=" + testNameId.stringId.urlencode()) }
        //     userId?.let { params.add("userId=" + userId.stringId.urlencode()) }
        //     modId?.let { params.add("modId=" + modId.stringId.urlencode()) }
        //     personal?.let { params.add("personal=" + if (personal) "true" else "false") }
        //     buildTypeId?.let { params.add("buildTypeId=" + buildTypeId.stringId.urlencode()) }
        //     branch?.let { params.add("branch=" + branch.urlencode()) }

        //     return "$serverUrl/$pageName" +
        //             if (params.isNotEmpty()) "?${params.joinToString(" & ")}" else ""
        // }
        internal string GetUserUrlPage(
            string pageName,
            string tab = null,
            ProjectId? projectId = null,
            BuildId? buildId = null,
            TestId? testNameId = null,
            UserId? userId = null,
            ChangeId? modId = null,
            bool? personal = null,
            BuildConfigurationId? buildTypeId = null,
            string branch = null)
        {
            var param = new List<string>();
            
            if (tab != null)
                param.Add($"tab={WebUtility.UrlEncode(tab)}");
            if (projectId.HasValue)
                param.Add($"projectId={WebUtility.UrlEncode(projectId.Value.stringId)}");
            if (buildId.HasValue)
                param.Add($"buildId={WebUtility.UrlEncode(buildId.Value.stringId)}");
            if (testNameId.HasValue)
                param.Add($"testNameId={WebUtility.UrlEncode(testNameId.Value.stringId)}");
            if (userId.HasValue)
                param.Add($"userId={WebUtility.UrlEncode(userId.Value.stringId)}");
            if (modId.HasValue)
                param.Add($"modId={WebUtility.UrlEncode(modId.Value.stringId)}");
            if (personal.HasValue)
            {
                var personalStr = personal.Value ? "true" : "false";
                param.Add($"personal={personalStr}");
            }
            if (buildTypeId.HasValue)
                param.Add($"buildTypeId={WebUtility.UrlEncode(buildTypeId.Value.stringId)}");
            if (!String.IsNullOrEmpty(branch))
                param.Add($"branch={WebUtility.UrlEncode(branch)}");

            var paramStr = param.IsNotEmpty()
                ? String.Join("&", param)
                : String.Empty;

            return $"{this.ServerUrl}/{pageName}?{paramStr}";
        }
    }

}