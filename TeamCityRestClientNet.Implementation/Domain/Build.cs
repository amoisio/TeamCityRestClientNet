using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nito.AsyncEx;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Tools;

namespace TeamCityRestClientNet.Domain
{
    class Build : Base<BuildDto>, IBuild
    {
        private Build(BuildDto fullDto, TeamCityServer instance)
            : base(fullDto, instance)
        {
            this.Changes = new AsyncLazy<List<IChange>>(async () 
                => {
                var changes = await instance.Service.Changes(
                    $"build:{IdString}",
                    "change(id,version,username,user,date,comment,vcsRootInstance)").ConfigureAwait(false);

                var tasks = changes.Items
                    ?.Select(dto => Change.Create(dto, true, instance));

                return (await Task.WhenAll(tasks).ConfigureAwait(false))
                    .ToList() 
                    ?? throw new NullReferenceException();
            });

            this.SnapshotDependencies = new AsyncLazy<List<IBuild>>(async () 
                => {
                    var tasks = fullDto.SnapshotDependencies
                        ?.Items
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

        public static async Task<IBuild> Create(string idString, TeamCityServer instance)
        {
            var dto = await instance.Service.Build(idString).ConfigureAwait(false);

            if (dto.BuildType == null)
            {
                dto.BuildType = new BuildTypeDto();
            }

            if (String.IsNullOrEmpty(dto.BuildType.Name))
            {
                dto.BuildType.Name = (await instance.BuildTypes.ById(new Id(dto.BuildTypeId)).ConfigureAwait(false)).Name;
            }

            return new Build(dto, instance);
        }

        public Id BuildTypeId => new Id(this.Dto.BuildTypeId);
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
        public override string Name => Dto.BuildType.Name;
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
                ?.Select(rev => new Revision(rev, Instance))
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

        public async IAsyncEnumerable<ITestRun> TestRuns(TestStatus? status = null)
        {
            var locator = Instance.TestRuns.ForBuild(Id);
            if (status != null)
                locator.WithStatus(status.Value);

            await foreach (var testRun in locator.All())
            {
                yield return testRun;
            } 
        }

        public IAsyncEnumerable<IBuildProblemOccurrence> BuildProblems() {
            var sequence = new Paged<IBuildProblemOccurrence, BuildProblemOccurrenceListDto>(
                Instance,
                async () => await Service.ProblemOccurrences(
                    $"build:(id:{Id})",
                    "$long,problemOccurrence($long)").ConfigureAwait(false)
                ,
                async (list) =>
                {
                    var occurrences = list.Items.Select(dto => new BuildProblemOccurrence(dto, Instance));
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
            await Service.CancelBuild(Id.StringId, request).ConfigureAwait(false);
        }

        private const int ARTIFACT_BUFFER = 512 * 1024;
        public async Task DownloadArtifact(string artifactPath, Stream output)
        {
            // LOG.info("Downloading artifact '$artifactPath' from build ${getHomeUrl()}")
            try
            {
                var stream = await OpenArtifactStream(artifactPath).ConfigureAwait(false);
                await stream.CopyToAsync(output, ARTIFACT_BUFFER).ConfigureAwait(false);
            }
            finally
            {
                // LOG.debug("Artifact '$artifactPath' from build ${getHomeUrl()} downloaded")
            }
        }

        public async Task<Stream> OpenArtifactStream(string artifactPath)
        {
            return await Service.ArtifactContent(Id.StringId, artifactPath).ConfigureAwait(false);
        }

        public async Task DownloadArtifact(string artifactPath, FileInfo outputFile)
        {
            // LOG.info("Downloading artifact '$artifactPath' from build ${getHomeUrl()} to $output")
            try
            {
                var stream = await OpenArtifactStream(artifactPath).ConfigureAwait(false);
                using var fileStream = outputFile.Open(FileMode.Create);
                await stream.CopyToAsync(fileStream, ARTIFACT_BUFFER).ConfigureAwait(false);
            }
            catch (Exception)
            {
                if (outputFile.Exists)
                    outputFile.Delete();
                throw;
            }
            finally
            {
                // LOG.debug("Artifact '$artifactPath' from build ${getHomeUrl()} downloaded to $output")
            }
        }

        public async Task DownloadArtifacts(string pattern, DirectoryInfo outputDir)
        {
            var artifacts = await GetArtifacts(recursive: true).ConfigureAwait(false);
            var regex = ConvertToRegex(pattern);
            var matches = artifacts.Where(art => regex.IsMatch(art.FullName));
            if (!matches.Any()) 
            {
                var available = artifacts
                    .Select(art => art.FullName)
                    .Aggregate((a, b) => $"{a},{b}");
                
                throw new TeamCityQueryException($"No artifacts matching {pattern} are found in build {BuildNumber}. Available artifacts: {available}.");
            }

            if (!outputDir.Exists)
                outputDir.Create();

            foreach(var match in matches) {
                var path = Path.Join(outputDir.ToString(), match.Name);
                await match.Download(new FileInfo(path)).ConfigureAwait(false);
            }
        }

        private Regex ConvertToRegex(string pattern)
        {
            return new Regex(pattern.Replace(".", "\\.").Replace("*", ".*").Replace("?", "."));
        }

        public async Task DownloadBuildLog(FileInfo outputFile)
        {
            // LOG.info("Downloading build log from build ${getHomeUrl()} to $output")
            var logStream = await Service.BuildLog(Id.StringId).ConfigureAwait(false);
            await SaveToFile(logStream, outputFile).ConfigureAwait(false);
            // LOG.debug("Build log from build ${getHomeUrl()} downloaded to $output")
        }

        private async Task SaveToFile(Stream logStream, FileInfo outputFile)
        {
            using var fileStream = outputFile.Open(FileMode.Create);
            await logStream.CopyToAsync(fileStream, ARTIFACT_BUFFER).ConfigureAwait(false);
        }

        public async Task<IBuildArtifact> FindArtifact(string pattern, string parentPath = "")
            => await FindArtifact(pattern, parentPath, false).ConfigureAwait(false);

        public async Task<IBuildArtifact> FindArtifact(string pattern, string parentPath = "", bool recursive = false)
        {
            var artifacts = await GetArtifacts(parentPath, recursive).ConfigureAwait(false);
            var regex = ConvertToRegex(pattern);
            var matches = artifacts.Where(art => regex.IsMatch(art.Name));
            if (!matches.Any())
            {
                var available = artifacts
                    .Select(art => art.Name)
                    .Aggregate((a, b) => $"{a},{b}");

                throw new TeamCityQueryException($"Artifact {pattern} not found in build {BuildNumber}. Available artifacts: {available}.");
            }

            if (matches.Count() > 1) 
            {
                var names = matches
                    .Select(art => art.Name)
                    .Aggregate((a, b) => $"{a},{b}");

                throw new TeamCityQueryException($"Several artifacts matching {pattern} are found in build {BuildNumber}: {names}.");
            }

            return matches.Single();
        }

        public async Task<List<IBuildArtifact>> GetArtifacts(
            string parentPath = "", 
            bool recursive = false, 
            bool hidden = false)
        {
            var locator = $"recursive:{(recursive ? "true" : "false")},hidden:{(hidden ? "true" : "false")}";
            var fields = $"file({ArtifactFileDto.FIELDS})";
            var artifacts = await Service
                .ArtifactChildren(Id.StringId, parentPath, locator, fields)
                .ConfigureAwait(false);

            return artifacts.File
                .Where(file => !String.IsNullOrEmpty(file.FullName) && !String.IsNullOrEmpty(file.ModificationTime))
                .Select(file => new BuildArtifact(
                    this, 
                    file.Name, 
                    file.FullName, 
                    file.Size, 
                    Utilities.ParseTeamCity(file.ModificationTime).Value))
                .ToList<IBuildArtifact>();
        }

        public string GetHomeUrl()
            => Instance.GetUserUrlPage("viewLog.html", buildId: Id);

        public async Task<List<IParameter>> GetResultingParameters()
        {
            var props = await Service
                .ResultingProperties(Id.StringId)
                .ConfigureAwait(false);
            return props.Property
                .Select(prop => new Parameter(prop))
                .ToList<IParameter>();
        }

        public async Task Pin(string comment = "pinned via REST API")
        {
            //         LOG.info("Pinning build ${getHomeUrl()}")
            await Service.Pin(IdString, comment).ConfigureAwait(false);
        }

        public async Task ReplaceTags(List<string> tags)
        {
            //         LOG.info("Replacing tags of build ${getHomeUrl()} with ${tags.joinToString(", ")}")
            var tagDtos = tags.Select(tag => new TagDto { Name = tag }).ToList();
            await Service
                .ReplaceTags(IdString, new TagsDto { Tag = tagDtos })
                .ConfigureAwait(false);
        }

        public async Task SetComment(string comment)
        {
        //         LOG.info("Adding comment $comment to build ${getHomeUrl()}")
            await Service.SetComment(IdString, comment).ConfigureAwait(false);
        }

        public override string ToString()
            => $"Build(id={Id}, buildTypeId={BuildTypeId}, buildNumber={BuildNumber}, status={Status}, branch={Branch})";

        public async Task Unpin(string comment = "unpinned via REST API")
        {
        //         LOG.info("Unpinning build ${getHomeUrl()}")
            await Service.Unpin(IdString).ConfigureAwait(false);
        }
    }
}
