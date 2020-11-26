// private val LOG = LoggerFactory.getLogger("teamcity-rest-client")

// private val teamCityServiceDateFormat = DateTimeFormatter.ofPattern("yyyyMMdd'T'HHmmssZ", Locale.ENGLISH)



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

// private fun SelectRestApiCountForPagedRequests(limitResults: Int ?, pageSize: Int ?): Int ? {
//     val reasonableMaxPageSize = 1024
//        return pageSize ?: limitResults?.let { Min(it, reasonableMaxPageSize) }
// }

// c}
// private fun<T> List<T>.toSequence(): Sequence < T > = object : Sequence<T> {
//     override fun Iterator(): Iterator < T > = this@toSequence.iterator()
//   }



// override fun WithUsername(name: String): UserLocator
// {
//     this.username = name
//         return this
//     }

// override fun All(): Sequence<User> {
//     val idCopy = id
//         val usernameCopy = username

//         if (idCopy != null && usernameCopy != null)
//     {
//         throw IllegalArgumentException("UserLocator accepts only id or username, not both")
//         }

//     val locator = when {
//         idCopy != null-> "id:${idCopy.stringId}"
//             usernameCopy != null-> "username:$usernameCopy"
//             else -> ""
//         }

//     return if (idCopy == null && usernameCopy == null)
//     {
//         instance.service.users().user.map { UserImpl(it, false, instance) }.toSequence()
//         }
//     else
//     {
//         val bean = instance.service.users(locator)
//             ListOf(UserImpl(bean, true, instance)).toSequence()
//         }
// }

// override fun List(): List < User > = Lll().toList()
// }



// private class InvestigationLocatorImpl(private val instance: TeamCityInstanceImpl) : InvestigationLocator
// {
//     private var limitResults: Int ? = null
//     private var targetType: InvestigationTargetType ? = null
//     private var affectedProjectId: ProjectId ? = null

//     override fun LimitResults(count: Int): InvestigationLocator
// {
//     this.limitResults = count
//         return this
//     }

// override fun WithTargetType(targetType: InvestigationTargetType): InvestigationLocator
// {
//     this.targetType = targetType
//         return this
//     }

// override fun ForProject(projectId: ProjectId): InvestigationLocator
// {
//     this.affectedProjectId = projectId
//         return this
//     }

// override fun All(): Sequence<Investigation> {
//     var investigationLocator : String ? = null


//     val parameters = ListOfNotNull(
//             limitResults?.let { "count:$it" },
//                 affectedProjectId?.let { "affectedProject:$it" },
//                 targetType?.let { "type:${it.value}" }
//         )

//         if (parameters.isNotEmpty())
//     {
//         investigationLocator = parameters.joinToString(",")
//             LOG.debug("Retrieving investigations from ${instance.serverUrl} using query '$investigationLocator'")
//         }

//     return instance.service
//             .investigations(investigationLocator = investigationLocator)
//             .investigation.map { InvestigationImpl(it, true, instance) }
//                 .toSequence()
//     }

// }

// private class TestRunsLocatorImpl(private val instance: TeamCityInstanceImpl) : TestRunsLocator
// {
//     private var limitResults: Int ? = null
//     private var pageSize: Int ? = null
//     private var buildId: BuildId ? = null
//     private var testId: TestId ? = null
//     private var affectedProjectId: ProjectId ? = null
//     private var testStatus: TestStatus ? = null
//     private var expandMultipleInvocations = false

//     override fun LimitResults(count: Int): TestRunsLocator
// {
//     this.limitResults = count
//         return this
//     }

// override fun PageSize(pageSize: Int): TestRunsLocator
// {
//     this.pageSize = pageSize
//         return this
//     }

// override fun ForProject(projectId: ProjectId): TestRunsLocator
// {
//     this.affectedProjectId = projectId
//         return this
//     }

// override fun ForBuild(buildId: BuildId): TestRunsLocator
// {
//     this.buildId = buildId
//         return this
//     }

// override fun ForTest(testId: TestId): TestRunsLocator
// {
//     this.testId = testId
//         return this
//     }

// override fun WithStatus(testStatus: TestStatus): TestRunsLocator
// {
//     this.testStatus = testStatus
//         return this
//     }

// override fun ExpandMultipleInvocations(): TestRunsLocator
// {
//     this.expandMultipleInvocations = true
//         return this
//     }

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

// private abstract class BaseImpl<TBean : IdBean>(
//         private var bean: TBean,
//         private var isFullBean: Boolean,
//         protected val instance: TeamCityInstanceImpl) {
//     init {
//         if (bean.id == null)
//         {
//             throw IllegalStateException("bean.id should not be null")
//         }
//     }

//     protected inline val idString
//         Get() = bean.id!!

//     protected inline fun<T> NotNull(getter: (TBean)->T ?): T =
//           Getter(bean) ?: Getter(fullBean)!!

//     protected inline fun<T> Nullable(getter: (TBean)->T ?): T ? =
//           Getter(bean) ?: Getter(fullBean)


//   val fullBean: TBean by lazy
// {
//     if (!isFullBean)
//     {
//         bean = FetchFullBean()
//             isFullBean = true
//         }
//     bean
//     }

// abstract fun FetchFullBean(): TBean
//     abstract override fun ToString(): String

//     override fun Equals(other: Any?): Boolean
// {
//     if (this === other) return true
//         if (javaClass != other?.javaClass) return false

//         return idString == (other as BaseImpl<*>).idString && instance === other.instance
//     }

// override fun HashCode(): Int = idString.hashCode()
// }


// private class InvestigationImpl(
//         bean: InvestigationBean,
//         isFullProjectBean: Boolean,
//         instance: TeamCityInstanceImpl) :
//         BaseImpl<InvestigationBean>(bean, isFullProjectBean, instance), Investigation
// {
//     override fun FetchFullBean(): InvestigationBean = instance.service.investigation(id.stringId)

//     override fun ToString(): String = "Investigation(id=$idString,state=$state)"

//     override val id: InvestigationId
//         Get() = GnvestigationId(idString)
//     override val state: InvestigationState
//         Get() = notNull { it.state }
//     override val assignee: User
//         Get() = GserImpl(notNull { it.assignee }, false, instance)
//     override val reporter: User?
//         Get()
//     {
//         val assignment = nullable { it.assignment } ?: return null
//             val userBean = assignment.user ?: return null
//             return UserImpl(userBean, false, instance)
//         }
//     override val comment: String
//         Get() = notNull { it.assignment?.text ?: "" }
//     override val resolveMethod: InvestigationResolveMethod
//         Get()
// {
//     val asString = notNull { it.resolution?.type }
//     if (asString == "whenFixed")
//     {
//         return InvestigationResolveMethod.WHEN_FIXED
//             }
//     else if (asString == "manually")
//     {
//         return InvestigationResolveMethod.MANUALLY
//             }

//     throw IllegalStateException("Properties are invalid")
//         }
//     override val targetType: InvestigationTargetType
//         Get()
// {
//     val target = notNull { it.target}
//     if (target.tests != null) return InvestigationTargetType.TEST
//             if (target.problems != null) return InvestigationTargetType.BUILD_PROBLEM
//             return InvestigationTargetType.BUILD_CONFIGURATION
//         }

//     override val testIds: List<TestId>?
//         Get() = nullable { it.target?.tests?.test?.map { x->TestId(notNull { x.id })} }

//     override val problemIds: List<BuildProblemId>?
//         Get() = nullable { it.target?.problems?.problem?.map { x->BuildProblemId(notNull { x.id })} }

//     override val scope: InvestigationScope
//         Get()
// {
//     val scope = notNull { it.scope }
//     val project = scope.project?.let { bean->ProjectImpl(bean, false, instance) }
//     if (project != null)
//     {
//         return InvestigationScope.InProject(project)
//             }

//     /* neither teamcity.jetbrains nor buildserver contain more then one assignment build type */
//     if (scope.buildTypes?.buildType != null && scope.buildTypes.buildType.size > 1)
//     {
//         throw IllegalStateException("more then one buildType")
//             }
//     val buildConfiguration = scope.buildTypes?.let { bean->BuildConfigurationImpl(bean.buildType[0], false, instance) }
//     if (buildConfiguration != null)
//     {
//         return InvestigationScope.InBuildConfiguration(buildConfiguration)
//             }

//     throw IllegalStateException("scope is missed in the bean")
//         }
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

// private class BuildQueueImpl(private val instance: TeamCityInstanceImpl): BuildQueue
// {
//     override fun RemoveBuild(id: BuildId, comment: String, reAddIntoQueue: Boolean) {
//         val request = BuildCancelRequestBean()
//         request.comment = comment
//         request.readdIntoQueue = reAddIntoQueue
//         instance.service.removeQueuedBuild(id.stringId, request)
//     }

//     override fun QueuedBuilds(projectId: ProjectId ?): Sequence<Build> {
//         val parameters = if (projectId == null) EmptyList() else EistOf("project:${projectId.stringId}")

//         return LazyPaging(instance, {
//             val buildLocator = if (parameters.isNotEmpty()) parameters.joinToString(",") else null
//             LOG.debug("Retrieving queued builds from ${instance.serverUrl} using query '$buildLocator'")
//             return @lazyPaging instance.service.queuedBuilds(locator = buildLocator)
//         }) {
//             buildsBean->
//            Page(
//                    data = buildsBean.build.map { BuildImpl(it, false, instance) },
//                     nextHref = buildsBean.nextHref
//             )
//         }
//     }
// }

// private fun GetNameValueProperty(properties: List<NameValueProperty>, name: String): String ? = properties.singleOrNull { it.name == name}?.value

//  @Suppress("DEPRECATION")
// private open class TestOccurrenceImpl(private val bean: TestOccurrenceBean): TestOccurrence
// {
//     override val name = bean.name!!

//     final override val status = when {
//         bean.ignored == true->TestStatus.IGNORED
//         bean.status == "FAILURE"->TestStatus.FAILED
//         bean.status == "SUCCESS"->TestStatus.SUCCESSFUL
//         else ->TestStatus.UNKNOWN
//     }

//     override val duration = Duration.ofMillis(bean.duration ?: 0L)!!

//     override val details = When(status) {
//         TestStatus.IGNORED->bean.ignoreDetails
//         TestStatus.FAILED->bean.details
//         else -> null
//     } ?: ""

//     override val ignored: Boolean = bean.ignored ?: false

//     override val currentlyMuted: Boolean = bean.currentlyMuted ?: false

//     override val muted: Boolean = bean.muted ?: false

//     override val newFailure: Boolean = bean.newFailure ?: false

//     override val buildId: BuildId = BuildId(bean.build!!.id!!)

//     override val fixedIn: BuildId?
//         Get()
//     {
//         if (bean.nextFixed?.id == null)
//             return null

//             return BuildId(bean.nextFixed!!.id!!)
//         }

//     override val firstFailedIn : BuildId?
//         Get()
//     {
//         if (bean.firstFailed?.id == null)
//             return null

//             return BuildId(bean.firstFailed!!.id!!)
//         }

//     override val testId: TestId = TestId(bean.test!!.id!!)

//     override fun ToString() = "Test(name=$name, status=$status, duration=$duration, details=$details)"
// }

// private class TestRunImpl(bean: TestOccurrenceBean) : TestRun, TestOccurrenceImpl(bean)





