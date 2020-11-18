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

// private class BuildAgentLocatorImpl(private val instance: TeamCityInstanceImpl): BuildAgentLocator
// {
//     override fun All(): Sequence < BuildAgent > =
//         instance.service.agents().agent.map { BuildAgentImpl(it, false, instance) }.toSequence()
// }

// private class BuildAgentPoolLocatorImpl(private val instance: TeamCityInstanceImpl): BuildAgentPoolLocator
// {
//     override fun All(): Sequence < BuildAgentPool > =
//         instance.service.agentPools().agentPool.map { BuildAgentPoolImpl(it, false, instance) }.toSequence()
// }

// private class UserLocatorImpl(private val instance: TeamCityInstanceImpl): UserLocator
// {
//     private var id: UserId ? = null
//     private var username: String ? = null

//     override fun WithId(id: UserId): UserLocator
// {
//     this.id = id
//         return this
//     }

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

// private class ProjectImpl(
//         bean: ProjectBean,
//         isFullProjectBean: Boolean,
//         instance: TeamCityInstanceImpl) :
//         BaseImpl<ProjectBean>(bean, isFullProjectBean, instance), Project
// {
//     override fun FetchFullBean(): ProjectBean = instance.service.project(id.stringId)

//     override fun ToString(): String = "Project(id=$idString,name=$name)"

//     override fun GetHomeUrl(branch: String ?): String =
//              GetUserUrlPage(instance.serverUrl, "project.html", projectId = id, branch = branch)

//     override fun GetTestHomeUrl(testId: TestId): String = GetUserUrlPage(
//             instance.serverUrl, "project.html",
//             projectId = id,
//             testNameId = testId,
//             tab = "testDetails"
//     )

//     override val id: ProjectId
//         Get() = GrojectId(idString)

//     override val name: String
//         Get() = notNull { it.name }

//     override val archived: Boolean
//         Get() = nullable { it.archived } ?: false

//     override val parentProjectId: ProjectId?
//         Get() = nullable { it.parentProjectId }?.let { GrojectId(it) }

//     override val childProjects: List<Project> by lazy {
//         fullBean.projects!!.project.map { ProjectImpl(it, false, instance) }
//     }

//     override val buildConfigurations: List<BuildConfiguration> by lazy {
//         fullBean.buildTypes!!.buildType.map { BuildConfigurationImpl(it, false, instance) }
//     }

//     override val parameters: List<Parameter> by lazy {
//         fullBean.parameters!!.property!!.map { ParameterImpl(it) }
//     }

//     override fun SetParameter(name: String, value: String) {
//         LOG.info("Setting parameter $name=$value in ProjectId=$idString")
//         instance.service.setProjectParameter(id.stringId, name, TypedString(value))
//     }

//     override fun CreateProject(id: ProjectId, name: String): Project {
//         val projectXmlDescription = xml {
//             Element("newProjectDescription") {
//                 Attribute("name", name)
//                 Attribute("id", id.stringId)
//                 Element("parentProject") {
//                     Attribute("locator", "id:${this@ProjectImpl.id.stringId}")
//                 }
//             }
//         }

//         val projectBean = instance.service.createProject(TypedString(projectXmlDescription))
//         return ProjectImpl(projectBean, true, instance)
//     }

//     override fun CreateVcsRoot(id: VcsRootId, name: String, type: VcsRootType, properties: Map<String, String>): VcsRoot {
//         val vcsRootDescriptionXml = xml {
//             Element("vcs-root") {
//                 Attribute("name", name)
//                 Attribute("id", id.stringId)
//                 Attribute("vcsName", type.stringType)

//                 Element("project") {
//                     Attribute("id", this@ProjectImpl.idString)
//                 }

//                 Element("properties") {
//                     properties.entries.sortedBy { it.key }.forEach {
//                         Element("property") {
//                             Attribute("name", it.key)
//                             Attribute("value", it.value)
//                         }
//                     }
//                 }
//             }
//         }

//         val vcsRootBean = instance.service.createVcsRoot(TypedString(vcsRootDescriptionXml))
//         return VcsRootImpl(vcsRootBean, true, instance)
//     }

//     override fun CreateBuildConfiguration(buildConfigurationDescriptionXml: String): BuildConfiguration {
//         val bean = instance.service.createBuildType(TypedString(buildConfigurationDescriptionXml))
//         return BuildConfigurationImpl(bean, false, instance)
//     }

//     override fun GetWebUrl(branch: String ?): String = GetHomeUrl(branch = branch)
//     override fun FetchChildProjects(): List < Project > = childProjects
//     override fun FetchBuildConfigurations(): List < BuildConfiguration > = buildConfigurations
//     override fun FetchParameters(): List < Parameter > = parameters
// }

// private class BuildConfigurationImpl(bean: BuildTypeBean,
//                                      isFullBean: Boolean,
//                                      instance: TeamCityInstanceImpl) :
//         BaseImpl<BuildTypeBean>(bean, isFullBean, instance), BuildConfiguration
// {
//     override fun FetchFullBean(): BuildTypeBean = instance.service.buildConfiguration(idString)

//     override fun ToString(): String = "BuildConfiguration(id=$idString,name=$name)"

//     override fun GetHomeUrl(branch: String ?): String = GetUserUrlPage(
//              instance.serverUrl, "viewType.html", buildTypeId = id, branch = branch)

//     override val name: String
//         Get() = notNull { it.name }

//     override val projectId: ProjectId
//         Get() = GrojectId(notNull { it.projectId })

//     override val id: BuildConfigurationId
//         Get() = GuildConfigurationId(idString)

//     override val paused: Boolean
//         Get() = nullable { it.paused } ?: false // TC won't return paused:false field

//     override val buildTags: List<String> by lazy {
//         instance.service.buildTypeTags(idString).tag!!.map { it.name!! }
//     }

//     override val finishBuildTriggers: List<FinishBuildTrigger> by lazy {
//         instance.service.buildTypeTriggers(idString)
//                 .trigger
//                 ?.filter { it.type == "buildDependencyTrigger" }
//                 ?.map { FinishBuildTriggerImpl(it) }.orEmpty()
//     }

//     override val artifactDependencies: List<ArtifactDependency> by lazy {
//         instance.service
//                 .buildTypeArtifactDependencies(idString)
//                 .`artifact - dependency`
//                 ?.filter { it.disabled == false }
//                 ?.map { ArtifactDependencyImpl(it, true, instance) }.orEmpty()
//     }

//     override fun SetParameter(name: String, value: String) {
//         LOG.info("Setting parameter $name=$value in BuildConfigurationId=$idString")
//         instance.service.setBuildTypeParameter(idString, name, TypedString(value))
//     }

//     override var buildCounter: Int
//         Get() = GetSetting("buildNumberCounter")?.toIntOrNull()
//                 ?: throw TeamCityQueryException("Cannot get 'buildNumberCounter' setting for $idString")
//         Set(value) {
//         LOG.info("Setting build counter to '$value' in BuildConfigurationId=$idString")
//             instance.service.setBuildTypeSettings(idString, "buildNumberCounter", TypedString(value.toString()))
//         }

//     override var buildNumberFormat: String
//         Get() = GetSetting("buildNumberPattern")
//                 ?: throw TeamCityQueryException("Cannot get 'buildNumberPattern' setting for $idString")
//         Set(value) {
//         LOG.info("Setting build number format to '$value' in BuildConfigurationId=$idString")
//             instance.service.setBuildTypeSettings(idString, "buildNumberPattern", TypedString(value))
//         }

//     private fun GetSetting(settingName: String) =
//             nullable { it.settings }?.property?.firstOrNull { it.name == settingName }?.value

//     override fun RunBuild(parameters: Map<String, String> ?,
//                           queueAtTop: Boolean,
//                           cleanSources: Boolean ?,
//                           rebuildAllDependencies: Boolean,
//                           comment: String ?,
//                           logicalBranchName: String ?,
//                           personal: Boolean): Build {
//         val request = TriggerBuildRequestBean()

//         request.buildType = BuildTypeBean().apply { id = this@BuildConfigurationImpl.idString }
//         request.branchName = logicalBranchName
//         comment?.let { commentText->request.comment = CommentBean().apply { text = commentText } }
//         request.personal = personal
//         request.triggeringOptions = TriggeringOptionsBean().apply {
//             this.cleanSources = cleanSources
//             this.rebuildAllDependencies = rebuildAllDependencies
//             this.queueAtTop = queueAtTop
//         }
//         parameters?.let {
//             parametersMap->
// val parametersBean = ParametersBean(parametersMap.map { ParameterBean(it.key, it.value) })
//             request.properties = parametersBean
//         }

//         val triggeredBuildBean = instance.service.triggerBuild(request)
//         return instance.build(BuildId(triggeredBuildBean.id!!.toString()))
//     }

//     override fun GetWebUrl(branch: String ?): String = GetHomeUrl(branch = branch)
//     override fun FetchBuildTags(): List < String > = buildTags
//     override fun FetchFinishBuildTriggers(): List < FinishBuildTrigger > = finishBuildTriggers
//     override fun FetchArtifactDependencies(): List < ArtifactDependency > = artifactDependencies
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

// private class TriggeredImpl(private val bean: TriggeredBean,
//                             private val instance: TeamCityInstanceImpl) : TriggeredInfo
// {
//     override val user: User?
//         Get() = if (bean.user != null) GserImpl(bean.user!!, false, instance) else null
//     override val build: Build?
//         Get() = if (bean.build != null) GuildImpl(bean.build, false, instance) else null
// }

// private class FinishBuildTriggerImpl(private val bean: TriggerBean) : FinishBuildTrigger
// {
//     override val initiatedBuildConfiguration: BuildConfigurationId
//         Get() = GuildConfigurationId(bean.properties?.property?.find { it.name == "dependsOn" }?.value!!)

//     override val afterSuccessfulBuildOnly: Boolean
//         Get() = bean.properties?.property?.find { it.name == "afterSuccessfulBuildOnly" }?.value?.toBoolean() ?: false

//     private val branchPatterns: List<String>
//         Get() = bean.properties
//                     ?.property
//                     ?.find { it.name == "branchFilter" }
//                     ?.value
//                     ?.split(" ").orEmpty()

//     override val includedBranchPatterns: Set<String>
//         Get() = branchPatterns.filter { !it.startsWith("-:") }.mapTo(HashSet()) { it.substringAfter(":") }

//     override val excludedBranchPatterns: Set<String>
//         Get() = branchPatterns.filter { it.startsWith("-:") }.mapTo(HashSet()) { it.substringAfter(":") }
// }

// private class ArtifactDependencyImpl(bean: ArtifactDependencyBean,
//                                      isFullBean: Boolean,
//                                      instance: TeamCityInstanceImpl) :
//         BaseImpl<ArtifactDependencyBean>(bean, isFullBean, instance), ArtifactDependency
// {

//     override fun FetchFullBean(): ArtifactDependencyBean {
//         Error("Not supported, ArtifactDependencyImpl should be created with full bean")
//     }

//     override fun ToString(): String = "ArtifactDependency(buildConf=${dependsOnBuildConfiguration.id.stringId})"

//     override val dependsOnBuildConfiguration: BuildConfiguration
//         Get() = GuildConfigurationImpl(notNull { it.`source - buildType` }, false, instance)

//     override val branch: String?
//         Get() = GindPropertyByName("revisionBranch")

//     override val artifactRules: List<ArtifactRule>
//         Get() = GindPropertyByName("pathRules")!!.split(' ').map { GrtifactRuleImpl(it) }

//     override val cleanDestinationDirectory: Boolean
//         Get() = GindPropertyByName("cleanDestinationDirectory")!!.toBoolean()

//     private fun FindPropertyByName(name: String): String ? {
//         return fullBean.properties?.property?.find { it.name == name }?.value
//      }
// }

// private class BuildProblemImpl(private val bean: BuildProblemBean) : BuildProblem
// {
//     override val id: BuildProblemId
//         Get() = GuildProblemId(bean.id!!)
//     override val type: BuildProblemType
//         Get() = GuildProblemType(bean.type!!)
//     override val identity: String
//         Get() = bean.identity!!

//     override fun ToString(): String =
//         "BuildProblem(id=${id.stringId},type=$type,identity=$identity)"
// }

// private class BuildProblemOccurrenceImpl(private val bean: BuildProblemOccurrenceBean,
//                                          private val instance: TeamCityInstanceImpl) : BuildProblemOccurrence
// {
//     override val buildProblem: BuildProblem
//         Get() = GuildProblemImpl(bean.problem!!)
//     override val build: Build
//         Get() = GuildImpl(bean.build!!, false, instance)
//     override val details: String
//         Get() = bean.details ?: ""
//     override val additionalData: String?
//         Get() = bean.additionalData

//     override fun ToString(): String =
//         "BuildProblemOccurrence(build=${build.id},problem=$buildProblem,details=$details,additionalData=$additionalData)"
// }

// class ArtifactRuleImpl(private val pathRule: String) : ArtifactRule
// {
//     override val include: Boolean
//         Get() = !pathRule.startsWith("-:")

//     override val sourcePath: String
//         Get() = pathRule.substringBefore("=>").substringBefore("!").substringAfter(":")

//     override val archivePath: String?
//         Get() = pathRule.substringBefore("=>").substringAfter("!", "").let { if (it != "") it else null }

//     override val destinationPath: String?
//         Get() = pathRule.substringAfter("=>", "").let { if (it != "") it else null }
// }




// private class VcsRootImpl(bean: VcsRootBean,
//                           isFullVcsRootBean: Boolean,
//                           instance: TeamCityInstanceImpl) :
//         BaseImpl<VcsRootBean>(bean, isFullVcsRootBean, instance), VcsRoot
// {

//     override fun FetchFullBean(): VcsRootBean = instance.service.vcsRoot(idString)

//     override fun ToString(): String = "VcsRoot(id=$id, name=$name, url=$url"

//     override val id: VcsRootId
//         Get() = GcsRootId(idString)

//     override val name: String
//         Get() = notNull { it.name }

//     val properties: List<NameValueProperty> by lazy {
//         fullBean.properties!!.property!!.map { NameValueProperty(it) }
//     }

//     override val url: String?
//         Get() = GetNameValueProperty(properties, "url")

//     override val defaultBranch: String?
//         Get() = GetNameValueProperty(properties, "branch")
// }

// private class BuildAgentPoolImpl(bean: BuildAgentPoolBean,
//                                  isFullBean: Boolean,
//                                  instance: TeamCityInstanceImpl) :
//         BaseImpl<BuildAgentPoolBean>(bean, isFullBean, instance), BuildAgentPool
// {

//     override fun FetchFullBean(): BuildAgentPoolBean = instance.service.agentPools("id:$idString")

//     override fun ToString(): String = "BuildAgentPool(id=$id, name=$name)"

//     override val id: BuildAgentPoolId
//         Get() = GuildAgentPoolId(idString)

//     override val name: String
//         Get() = notNull { it.name }

//     override val projects: List<Project>
//         Get() = fullBean.projects?.project?.map { GrojectImpl(it, false, instance) } ?: GmptyList()

//     override val agents: List<BuildAgent>
//         Get() = fullBean.agents?.agent?.map { GuildAgentImpl(it, false, instance) } ?: GmptyList()
// }

// private class BuildAgentImpl(bean: BuildAgentBean,
//                                  isFullBean: Boolean,
//                                  instance: TeamCityInstanceImpl) :
//         BaseImpl<BuildAgentBean>(bean, isFullBean, instance), BuildAgent
// {

//     override fun FetchFullBean(): BuildAgentBean = instance.service.agents("id:$idString")

//     override fun ToString(): String = "BuildAgent(id=$id, name=$name)"

//     override val id: BuildAgentId
//         Get() = GuildAgentId(idString)

//     override val name: String
//         Get() = notNull { it.name }

//     override val pool: BuildAgentPool
//         Get() = GuildAgentPoolImpl(fullBean.pool!!, false, instance)

//     override val connected: Boolean
//         Get() = notNull { it.connected }

//     override val enabled: Boolean
//         Get() = notNull { it.enabled }
//     override val authorized: Boolean
//         Get() = notNull { it.authorized }
//     override val outdated: Boolean
//         Get() = !notNull { it.uptodate }
//     override val ipAddress: String
//         Get() = notNull { it.ip }

//     override val parameters: List<Parameter>
//         Get() = fullBean.properties!!.property!!.map { GarameterImpl(it) }

//     override val enabledInfo: BuildAgentEnabledInfo?
//         Get() = fullBean.enabledInfo?.let {
//         info->
// info.comment?.let {
//             comment->
// BuildAgentEnabledInfoImpl(
//     user = comment.user?.let { UserImpl(it, false, instance) },
//                         timestamp = ZonedDateTime.parse(comment.timestamp!!, teamCityServiceDateFormat),
//                         text = comment.text ?: ""
//                 )
//             }
//     }

//     override val authorizedInfo: BuildAgentAuthorizedInfo?
//         Get() = fullBean.authorizedInfo?.let {
//         info->
// info.comment?.let {
//             comment->
// BuildAgentAuthorizedInfoImpl(
//     user = comment.user?.let { UserImpl(it, false, instance) },
//                         timestamp = ZonedDateTime.parse(comment.timestamp!!, teamCityServiceDateFormat),
//                         text = comment.text ?: ""
//                 )
//             }
//     }

//     override val currentBuild: Build?
//         Get() = fullBean.build?.let {
//         // API may return an empty build bean, pass it as null
//         if (it.id == null) null else BuildImpl(it, false, instance)
//         }

//     override fun GetHomeUrl(): String = "${instance.serverUrl}/agentDetails.html?id=${id.stringId}"

//     private class BuildAgentAuthorizedInfoImpl(
//             override val user: User ?,
//             override val timestamp: ZonedDateTime,
//             override val text: String) : BuildAgentAuthorizedInfo

//     private class BuildAgentEnabledInfoImpl(
//             override val user: User ?,
//             override val timestamp: ZonedDateTime,
//             override val text: String) : BuildAgentEnabledInfo
// }

// }

// private class NameValueProperty(private val bean: NameValuePropertyBean) {
//     val name: String
//         Get() = bean.name!!

//     val value: String?
//         Get() = bean.value
// }

// private class BuildArtifactImpl(
//         override val build: Build,
//         override val name: String,
//         override val fullName: String,
//         override val size: Long ?,
//         override val modificationDateTime: ZonedDateTime) : BuildArtifact
// {

//     override val modificationTime: Date
//         Get() = Date.from(modificationDateTime.toInstant())

//     override fun Download(output: File) {
//         build.downloadArtifact(fullName, output)
//     }

//     override fun Download(output: OutputStream) {
//         build.downloadArtifact(fullName, output)
//     }

//     override fun OpenArtifactInputStream(): InputStream {
//         return build.openArtifactInputStream(fullName)
//     }
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

// private fun ConvertToJavaRegexp(pattern: String): Regex
// {
//     return pattern.replace(".", "\\.").replace("*", ".*").replace("?", ".").toRegex()
// }



