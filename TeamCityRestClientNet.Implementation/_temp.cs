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



// From TeamCityServer.cs
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


//     override fun GetWebUrl(projectId: ProjectId, branch: String ?): String =
//              Project(projectId).getHomeUrl(branch = branch)

//     override fun GetWebUrl(buildTypeId: BuildTypeId, branch: String ?): String =
//              BuildType(buildTypeId).getHomeUrl(branch = branch)

//     override fun GetWebUrl(buildId: BuildId): String =
//             Build(buildId).getHomeUrl()

//     override fun GetWebUrl(changeId: ChangeId, specificBuildTypeId: BuildTypeId ?, includePersonalBuilds: Boolean ?): String =
//               Change(changeId).getHomeUrl(
//                       specificBuildTypeId = specificBuildTypeId,
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
