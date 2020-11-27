// private val LOG = LoggerFactory.getLogger("teamcity-rest-client")

// private class RetryInterceptor : Interceptor
// {
//     private fun Response.retryRequired(): Boolean {
//         val code = code
//         if (code< 400) return false

//         // Do not retry non-GET methods, their result may be not idempotent
//         if (request.method != "GET") return false

//         return  code == HttpURLConnection.HTTP_CLIENT_TIMEOUT ||
//                 code == HttpURLConnection.HTTP_INTERNAL_ERROR ||
//                 code == HttpURLConnection.HTTP_BAD_GATEWAY ||
//                 code == HttpURLConnection.HTTP_UNAVAILABLE ||
//                 code == HttpURLConnection.HTTP_GATEWAY_TIMEOUT
// }

// override fun Intercept(chain: Interceptor.Chain): Response
// {
//     val request = chain.request()
//         var response = chain.proceed(request)

//         var tryCount = 0
//         while (response.retryRequired() && tryCount < 3)
//     {
//         tryCount++
//             LOG.warn("Request ${request.url} is not successful, $tryCount sec waiting [$tryCount retry]")
//             runCatching { response.close() }
//         Thread.sleep((tryCount * 1000).toLong())
//             response = chain.proceed(request)
//         }

//     return response
//     }
// }

// private fun Xml(init: XMLStreamWriter.()->Unit): String
// {
//     val stringWriter = StringWriter()
//     val xmlStreamWriter = XMLOutputFactory.newFactory().createXMLStreamWriter(stringWriter)
//     Init(xmlStreamWriter)
//     xmlStreamWriter.flush()
//     return stringWriter.toString()
// }

// private fun XMLStreamWriter.element(name: String, init: XMLStreamWriter.() -> Unit): XMLStreamWriter
// {
//     this.writeStartElement(name)
//     this.init()
//     this.writeEndElement()
//     return this
// }

// private fun XMLStreamWriter.attribute(name: String, value: String) = WriteAttribute(name, value)


// internal class TeamCityInstanceImpl(override val serverUrl: String,
//                                     val serverUrlBase: String,
//                                     private val authHeader: String ?,
//                                     logResponses: Boolean,
//                                     timeout: Long = 2,
//                                     unit: TimeUnit = TimeUnit.MINUTES
// ) : TeamCityInstance() {
//     override fun withLogResponses() = TeamCityInstanceImpl(serverUrl, serverUrlBase, authHeader, true)
//     override fun withTimeout(timeout: Long, unit: TimeUnit) = TeamCityInstanceImpl(serverUrl, serverUrlBase, authHeader, true, timeout, unit)

//     private val restLog = LoggerFactory.getLogger(LOG.name + ".rest")

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
//                                 override fun newThread(r: Runnable) = thread(
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

//     override fun close()
// {
//     fun catchAll(action: ()->Unit): Unit = try
//     {
//         action()
//         }
//     catch (t: Throwable) {
//         LOG.warn("Failed to close connection. ${t.message}", t)
//         }

//     catchAll { client.dispatcher.cancelAll() }
//     catchAll { client.dispatcher.executorService.shutdown() }
//     catchAll { client.connectionPool.evictAll() }
//     catchAll { client.cache?.close() }
//     }

//     override fun builds(): BuildLocator = BuildLocatorImpl(this)

//     override fun investigations(): InvestigationLocator = InvestigationLocatorImpl(this)

//     override fun build(id: BuildId): Build = BuildImpl(
//             BuildBean().also { it.id = id.stringId }, false, this)

//     override fun build(buildConfigurationId: BuildConfigurationId, number: String): Build ? =
//             BuildLocatorImpl(this).fromConfiguration(buildConfigurationId).withNumber(number).latest()

//     override fun buildConfiguration(id: BuildConfigurationId): BuildConfiguration =
//             BuildConfigurationImpl(BuildTypeBean().also { it.id = id.stringId }, false, this)

//     override fun vcsRoots(): VcsRootLocator = VcsRootLocatorImpl(this)

//     override fun vcsRoot(id: VcsRootId): VcsRoot = VcsRootImpl(service.vcsRoot(id.stringId), true, this)

//     override fun project(id: ProjectId): Project = ProjectImpl(ProjectBean().let { it.id = id.stringId; it }, false, this)

//     override fun rootProject(): Project = project(ProjectId("_Root"))

//     override fun user(id: UserId): User =
//             UserImpl(UserBean().also { it.id = id.stringId }, false, this)

//     override fun user(userName: String): User {
//         val bean = service.users("username:$userName")
//         return UserImpl(bean, true, this)
//     }

//     override fun users(): UserLocator = UserLocatorImpl(this)

//     override fun change(buildConfigurationId: BuildConfigurationId, vcsRevision: String): Change =
//             ChangeImpl(service.change(
//                     buildType = buildConfigurationId.stringId, version = vcsRevision), true, this)

//     override fun change(id: ChangeId): Change =
//             ChangeImpl(ChangeBean().also { it.id = id.stringId }, false, this)

//     override fun buildQueue(): BuildQueue = BuildQueueImpl(this)

//     override fun buildAgents(): BuildAgentLocator = BuildAgentLocatorImpl(this)

//     override fun buildAgentPools(): BuildAgentPoolLocator = BuildAgentPoolLocatorImpl(this)

//     override fun getWebUrl(projectId: ProjectId, branch: String ?): String =
//              project(projectId).getHomeUrl(branch = branch)

//     override fun getWebUrl(buildConfigurationId: BuildConfigurationId, branch: String ?): String =
//              buildConfiguration(buildConfigurationId).getHomeUrl(branch = branch)

//     override fun getWebUrl(buildId: BuildId): String =
//             build(buildId).getHomeUrl()

//     override fun getWebUrl(changeId: ChangeId, specificBuildConfigurationId: BuildConfigurationId ?, includePersonalBuilds: Boolean ?): String =
//               change(changeId).getHomeUrl(
//                       specificBuildConfigurationId = specificBuildConfigurationId,
//                       includePersonalBuilds = includePersonalBuilds
//               )

//     override fun queuedBuilds(projectId: ProjectId ?): List < Build > =
//              buildQueue().queuedBuilds(projectId = projectId).toList()

//     override fun testRuns(): TestRunsLocator = TestRunsLocatorImpl(this)
// }



// private class VcsRootLocatorImpl(private val instance: TeamCityInstanceImpl) : VcsRootLocator
// {
//     override fun All(): Sequence<VcsRoot> {
//         return LazyPaging(instance, {
//             LOG.debug("Retrieving vcs roots from ${instance.serverUrl}")
//             return @lazyPaging instance.service.vcsRoots()
//         }) {
//             vcsRootsBean->
//            Page(
//                    data = vcsRootsBean.`vcs - root`.map { VcsRootImpl(it, false, instance) },
//                     nextHref = vcsRootsBean.nextHref
//             )
//         }
//     }

//     override fun List(): List < VcsRoot > = Lll().toList()
// }