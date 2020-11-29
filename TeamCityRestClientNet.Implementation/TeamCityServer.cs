using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BAMCIS.Util.Concurrent;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Locators;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet
{
    class TeamCityServer : TeamCity
    {
        public const string TEAMCITY_DATETIME_FORMAT = "yyyyMMdd'T'HHmmssZ";
        public const string TEAMCITY_DEFAUL_LOCALE = "en-US";
        private readonly string _authHeader;
        private readonly bool _logResponses;
        private readonly TimeUnit _unit;
        private readonly long _timeout;

        internal TeamCityServer(
            string serverUrl,
            string serverUrlBase,
            string authHeader,
            bool logResponses,
            TimeUnit unit,
            long timeout = 2)
        {

            //     private val restLog = LoggerFactory.getLogger(LOG.name + ".rest")
            this.ServerUrl = serverUrl;
            this.ServerUrlBase = serverUrlBase;
            this._authHeader = authHeader;
            this._logResponses = logResponses;
            this._unit = unit;
            this._timeout = timeout;
        }

        public string ServerUrl { get; }

        public string ServerUrlBase { get; }

        public override async Task<IBuild> Build(BuildId id) 
            => await Domain.Build.Create(id.stringId, this).ConfigureAwait(false);

        public override async Task<IBuild> Build(BuildConfigurationId buildConfigurationId, string number)
            => await new BuildLocator(this)
            .FromConfiguration(buildConfigurationId)
            .WithNumber(number)
            .Latest()
            .ConfigureAwait(false);
        
        public override IBuildAgentLocator BuildAgents => new BuildAgentLocator(this);

        public override IBuildAgentPoolLocator BuildAgentPools => new BuildAgentPoolLocator(this);

        public override async Task<IBuildConfiguration> BuildConfiguration(string id) 
            => await Domain.BuildConfiguration.Create(id, this).ConfigureAwait(false);
        
        public override async Task<IBuildConfiguration> BuildConfiguration(BuildConfigurationId id)
            => await BuildConfiguration(id.stringId).ConfigureAwait(false);

        public override IBuildQueue BuildQueue => new BuildQueue(this);

        public override IBuildLocator Builds => new BuildLocator(this);
        public override async Task<IChange> Change(BuildConfigurationId buildConfigurationId, string vcsRevision)
        {
            var dto = await Service.Change(buildConfigurationId.stringId, vcsRevision).ConfigureAwait(false);
            return await Domain.Change.Create(dto, true, this).ConfigureAwait(false);
        }

        public override async Task<IChange> Change(ChangeId id) 
            => await Domain.Change.Create(new ChangeDto { Id = id.stringId }, false, this).ConfigureAwait(false);

        public override IInvestigationLocator Investigations 
            => new InvestigationLocator(this);

        public override async Task<IProject> Project(ProjectId id) 
            => await Domain.Project.Create(new ProjectDto { Id = id.stringId }, false, this).ConfigureAwait(false);

        public override IAsyncEnumerable<IBuild> QueuedBuilds(ProjectId projectId) 
            => new BuildQueue(this).QueuedBuilds(projectId);

        public override async Task<IProject> RootProject() 
            => await Project(new ProjectId("_Root")).ConfigureAwait(false);

        public override ITestRunsLocator TestRuns => new TestRunsLocator(this);

        public override async Task<IUser> User(UserId id)
            => await Domain.User.Create(id.stringId, this).ConfigureAwait(false);

        public override async Task<IUser> User(string userName)
        {
            var fullDto = await Service.Users($"username:{userName}").ConfigureAwait(false);
            return await Domain.User.Create(fullDto, true, this).ConfigureAwait(false);
        }
        public override IUserLocator Users => new UserLocator(this);
        //     override fun VcsRoot(id: VcsRootId): VcsRoot = VcsRootImpl(service.vcsRoot(id.stringId), true, this)
        public override async Task<IVcsRoot> VcsRoot(VcsRootId id)
        {
            throw new System.NotImplementedException();
        }
        //     override fun VcsRoots(): VcsRootLocator = VcsRootLocatorImpl(this)
        public override async Task<IVcsRootLocator> VcsRoots()
        {
            throw new System.NotImplementedException();
        }

        public override TeamCity WithLogResponses()
            => new TeamCityServer(ServerUrl, ServerUrlBase, _authHeader, true, TimeUnit.MINUTES);

        public override TeamCity WithTimeout(long timeout, TimeUnit unit)
            => new TeamCityServer(ServerUrl, ServerUrlBase, _authHeader, true, unit, timeout);

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
        public ITeamCityService Service => throw new NotImplementedException();


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

    public class TeamCityQueryException : TeamCityException
    {
        public TeamCityQueryException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }

    public class TeamCityConversationException : TeamCityException
    {
        public TeamCityConversationException(string message = null, Exception cause = null)
            : base(message, cause)
        {

        }
    }
}