using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class Build : Base<BuildDto>, IBuild
    {
        private Build(BuildDto fullDto, TeamCityInstance instance)
            : base(fullDto, instance)
        {
            this.Changes = new AsyncLazy<List<IChange>>(async () 
                => {
                var changes = await instance.Service.Changes(
                    $"build:{IdString}",
                    "change(id,version,username,user,date,comment,vcsRootInstance)").ConfigureAwait(false);

                var tasks = changes.Change
                    ?.Select(dto => Change.Create(dto, true, instance));

                return (await Task.WhenAll(tasks).ConfigureAwait(false))
                    .ToList() 
                    ?? throw new NullReferenceException();
            });

            this.SnapshotDependencies = new AsyncLazy<List<IBuild>>(async () 
                => {
                    var tasks = fullDto.SnapshotDependencies
                        ?.Build
                        ?.Select(dto => Build.Create(dto.Id, instance));
            
                    return tasks != null 
                        ? (await Task.WhenAll(tasks).ConfigureAwait(false))
                            .ToList<IBuild>()
                        : new List<IBuild>();
            });

            this.Agent = new AsyncLazy<IBuildAgent>(async () 
                => (Dto.Agent?.Id == null)
                    ? null
                    : await BuildAgent.Create(Dto.Agent.Id, Instance).ConfigureAwait(false)
                );
        }

        public static async Task<IBuild> Create(string idString, TeamCityInstance instance)
        {
            var dto = await instance.Service.Build(idString).ConfigureAwait(false);

            if (dto.BuildType == null)
            {
                dto.BuildType = new BuildTypeDto();
            }

            if (String.IsNullOrEmpty(dto.BuildType.Name))
            {
                dto.BuildType.Name = (await instance.BuildConfiguration(dto.BuildTypeId).ConfigureAwait(false)).Name;
            }

            return new Build(dto, instance);
        }

        public BuildId Id => new BuildId(this.IdString);
        public BuildConfigurationId BuildConfigurationId => new BuildConfigurationId(this.Dto.BuildTypeId);
        public string BuildNumber => this.Dto.Number;
        public BuildStatus? Status => this.Dto.Status;
        public IBranch Branch
        {
            get
            {
                var branchName = this.Dto.BranchName;
                var isDefaultBranch = this.Dto.DefaultBranch;
                return new Branch(branchName, isDefaultBranch ?? String.IsNullOrEmpty(branchName));
            }
        }
        public BuildState State
        {
            get
            {
                if (Enum.TryParse<BuildState>(this.Dto.State, true, out BuildState state))
                {
                    return state;
                }
                else
                {
                    return BuildState.UNKNOWN;
                }
            }
        }
        public bool Personal => this.Dto.Personal ?? false;
        public string Name => Dto.BuildType.Name;
        public IBuildCanceledInfo CanceledInfo
            => this.Dto.CanceledInfo != null
                ? new BuildCanceledInfo(this.Dto.CanceledInfo, Instance)
                : null;
        public IBuildCommentInfo Comment
            => this.Dto.Comment != null
                ? new BuildCommentInfo(this.Dto.Comment, Instance)
                : null;
        public bool? Composite => this.Dto.Composite;
        public string StatusText => this.Dto.StatusText;
        public DateTimeOffset QueuedDateTime
            => Utilities.ParseTeamCity(this.Dto.QueuedDate)
            ?? throw new NullReferenceException();
        public DateTimeOffset? StartDateTime => Utilities.ParseTeamCity(this.Dto.StartDate);
        public DateTimeOffset? FinishDateTime => Utilities.ParseTeamCity(this.Dto.FinishDate);
        public IBuildRunningInfo RunningInfo
            => this.Dto.RunningInfo != null
                ? new BuildRunningInfo(this.Dto.RunningInfo)
                : null;
        public List<IParameter> Parameters
            => this.Dto.Properties
                ?.Property
                ?.Select(prop => new Parameter(prop))
                .ToList<IParameter>()
                ?? throw new NullReferenceException();
        public List<string> Tags
            => Dto.Tags?.Tag
                ?.Where(tag => !String.IsNullOrEmpty(tag.Name))
                .Select(tag => tag.Name)
                .ToList()
                ?? new List<string>();
        public List<IRevision> Revisions
            => this.Dto.Revisions
                ?.Revision
                ?.Select(rev => new Revision(rev))
                .ToList<IRevision>()
                ?? throw new NullReferenceException();

        public AsyncLazy<List<IChange>> Changes { get; }
        public AsyncLazy<List<IBuild>> SnapshotDependencies { get; }
        public IPinInfo PinInfo 
            => Dto.PinInfo != null
                ? new PinInfo(Dto.PinInfo, Instance)
                : null;
        public ITriggeredInfo TriggeredInfo 
            => Dto.Triggered != null
                ? new Triggered(Dto.Triggered, Instance)
                : null;


        public AsyncLazy<IBuildAgent> Agent { get; }


        //     override fun Tests(status: TestStatus ?): Sequence < TestRun > = TestRuns(status)
        //     override fun TestRuns(status: TestStatus ?): Sequence < TestRun > = instance
        //              .testRuns()
        //              .forBuild(id)
        //              .let { if (status == null) it else it.withStatus(status) }
        //             .all()
        public IAsyncEnumerable<ITestRun> TestRuns(TestStatus? status = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IBuildProblemOccurrence> BuildProblems() {
            var sequence = new Paged<IBuildProblemOccurrence, BuildProblemOccurrencesDto>(
                Service,
                async () => await Service.ProblemOccurrences(
                    $"build:(id:{Id.stringId})",
                    "$long,problemOccurrence($long)").ConfigureAwait(false)
                ,
                async (list) =>
                {
                    var occurrences = list.ProblemOccurrence.Select(dto => new BuildProblemOccurrence(dto, Instance));
                    var page = new Page<IBuildProblemOccurrence>(occurrences.ToArray(), list.NextHref);
                    return await Task.FromResult(page);
                }
            );
            return sequence;
        }

        public async Task AddTag(string tag)
        {
            //         LOG.info("Adding tag $tag to build ${getHomeUrl()}")
            await Service.AddTag(IdString, tag).ConfigureAwait(false);
        }

        public async Task Cancel(string comment = "", bool reAddIntoQueue = false)
        {
            var request = new BuildCancelRequestDto
            {
                Comment = comment,
                ReaddIntoQueue = reAddIntoQueue
            };
            await Service.CancelBuild(Id.stringId, request).ConfigureAwait(false);
        }



        //         private fun downloadArtifactImpl(artifactPath: String, output: OutputStream)
        //         {
        //             openArtifactInputStreamImpl(artifactPath).use {
        //                 input->
        // output.use {
        //                     input.copyTo(output, bufferSize = 512 * 1024)
        //             }
        //             }
        //         }


    //     private fun openArtifactInputStreamImpl(artifactPath: String) : InputStream {
    //     val response = instance.service.artifactContent(id.stringId, artifactPath)
    //     return response.body.`in`()
    // }

        //     override fun DownloadArtifact(artifactPath: String, output: OutputStream) {
        //         LOG.info("Downloading artifact '$artifactPath' from build ${getHomeUrl()}")
        //         try
        //         {
        //             DownloadArtifactImpl(artifactPath, output)
        //         }
        //         finally
        //         {
        //             LOG.debug("Artifact '$artifactPath' from build ${getHomeUrl()} downloaded")
        //         }
        //     }

        public async Task<Stream> DownloadArtifact(string artifactPath, Stream output)
        {
            
            throw new NotImplementedException();
        }


        //     override fun DownloadArtifact(artifactPath: String, output: File) {
        //         LOG.info("Downloading artifact '$artifactPath' from build ${getHomeUrl()} to $output")
        //         try
        //         {
        //             output.parentFile.mkdirs()
        //             FileOutputStream(output).use {
        //                 DownloadArtifactImpl(artifactPath, it)
        //             }
        //         }
        //         catch (t: Throwable) {
        //             output.delete()
        //             throw t
        //         } finally
        //         {
        //             LOG.debug("Artifact '$artifactPath' from build ${getHomeUrl()} downloaded to $output")
        //         }
        //     }

        //     private fun DownloadArtifactImpl(artifactPath: String, output: OutputStream) {
        //         OpenArtifactInputStreamImpl(artifactPath).use {
        //             input->
        // output.use {
        //                 input.copyTo(output, bufferSize = 512 * 1024)
        //             }
        //         }
        //     }
        public Task DownloadArtifact(string artifactPath, FileInfo outputFile)
        {
            throw new NotImplementedException();
        }


        //     override fun DownloadArtifacts(pattern: String, outputDir: File) {
        //         val list = GetArtifacts(recursive = true)
        //         val regexp = ConvertToJavaRegexp(pattern)
        //         val matched = list.filter { regexp.matches(it.fullName) }
        //         if (matched.isEmpty())
        //         {
        //             val available = list.joinToString(",") { it.fullName }
        //             throw TeamCityQueryException("No artifacts matching $pattern are found in build $buildNumber. Available artifacts: $available.")
        //         }
        //         outputDir.mkdirs()
        //         matched.forEach {
        //             it.download(File(outputDir, it.name))
        //         }
        //     }


        public Task DownloadArtifacts(string pattern, DirectoryInfo outputDir)
        {
            throw new NotImplementedException();
        }


        //     override fun DownloadBuildLog(output: File) {
        //         LOG.info("Downloading build log from build ${getHomeUrl()} to $output")

        //         val response = instance.service.buildLog(id.stringId)
        //         SaveToFile(response, output)

        //         LOG.debug("Build log from build ${getHomeUrl()} downloaded to $output")
        //     }
        public Task DownloadBuildLog(FileInfo outputFile)
        {
            throw new NotImplementedException();
        }

        //     override fun FindArtifact(pattern: String, parentPath: String): BuildArtifact {
        //         return FindArtifact(pattern, parentPath, false)
        //     }

        public IBuildArtifact FindArtifact(string pattern, string parentPath = "")
        {
            throw new NotImplementedException();
        }


        //     override fun FindArtifact(pattern: String, parentPath: String, recursive: Boolean): BuildArtifact {
        //         val list = GetArtifacts(parentPath, recursive)
        //         val regexp = ConvertToJavaRegexp(pattern)
        //         val result = list.filter { regexp.matches(it.name) }
        //         if (result.isEmpty())
        //         {
        //             val available = list.joinToString(",") { it.name }
        //             throw TeamCityQueryException("Artifact $pattern not found in build $buildNumber. Available artifacts: $available.")
        //         }
        //         if (result.size > 1)
        //         {
        //             val names = result.joinToString(",") { it.name }
        //             throw TeamCityQueryException("Several artifacts matching $pattern are found in build $buildNumber: $names.")
        //         }
        //         return result.first()
        //     }
        public IBuildArtifact FindArtifact(string pattern, string parentPath = "", bool recursive = false)
        {
            throw new NotImplementedException();
        }




        //     override fun GetArtifacts(parentPath: String, recursive: Boolean, hidden: Boolean): List<BuildArtifact> {
        //         val locator = "recursive:$recursive,hidden:$hidden"
        //         val fields = "file(${ArtifactFileBean.FIELDS})"
        //         return instance.service.artifactChildren(id.stringId, parentPath, locator, fields).file
        //                 .filter { it.fullName != null && it.modificationTime != null }
        //                 .map { BuildArtifactImpl(this, it.name!!, it.fullName!!, it.size, ZonedDateTime.parse(it.modificationTime!!, teamCityServiceDateFormat)) }
        //     }
        public List<IBuildArtifact> GetArtifacts(string parentPath = "", bool recursive = false, bool hidden = false)
        {
            throw new NotImplementedException();
        }

        public string GetHomeUrl()
            => Instance.GetUserUrlPage("viewLog.html", buildId: Id);

        //     override fun GetResultingParameters(): List<Parameter> {
        //         return instance.service.resultingProperties(id.stringId).property!!.map { ParameterImpl(it) }
        //     }
        public List<IParameter> GetResultingParameters()
        {
            throw new NotImplementedException();
        }



        //     override fun OpenArtifactInputStream(artifactPath: String): InputStream {
        //         LOG.info("Opening artifact '$artifactPath' stream from build ${getHomeUrl()}")
        //         return OpenArtifactInputStreamImpl(artifactPath)
        //     }

        public Stream OpenArtifactInputStream(string artifactPath)
        {
            throw new NotImplementedException();
        }


        //     private fun OpenArtifactInputStreamImpl(artifactPath: String) : InputStream {
        //         val response = instance.service.artifactContent(id.stringId, artifactPath)
        //         return response.body.`in`()
        //     }



        //     override fun Pin(comment: String) {
        //         LOG.info("Pinning build ${getHomeUrl()}")
        //         instance.service.pin(idString, TypedString(comment))
        //     }
        public void Pin(string comment = "pinned via REST API")
        {
            throw new NotImplementedException();
        }



        //     override fun ReplaceTags(tags: List<String>) {
        //         LOG.info("Replacing tags of build ${getHomeUrl()} with ${tags.joinToString(", ")}")
        //         val tagBeans = tags.map { tag->TagBean().apply { name = tag } }
        //         instance.service.replaceTags(idString, TagsBean().apply { tag = tagBeans })
        //     }
        public void ReplaceTags(List<string> tags)
        {
            throw new NotImplementedException();
        }

        //     override fun SetComment(comment: String) {
        //         LOG.info("Adding comment $comment to build ${getHomeUrl()}")
        //         instance.service.setComment(idString, TypedString(comment))
        //     }

        public void SetComment(string comment)
        {
            throw new NotImplementedException();
        }



        //     override fun ToString(): String {
        //         return "Build{id=$id, buildConfigurationId=$buildConfigurationId, buildNumber=$buildNumber, status=$status, branch=$branch}"
        //     }
        public override string ToString()
        {
            throw new NotImplementedException();
        }



        //     override fun Unpin(comment: String) {
        //         LOG.info("Unpinning build ${getHomeUrl()}")
        //         instance.service.unpin(idString, TypedString(comment))
        //     }
        public void Unpin(string comment = "unpinned via REST API")
        {
            throw new NotImplementedException();
        }
    }
}