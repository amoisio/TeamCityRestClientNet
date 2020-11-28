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
