using System;
using System.IO;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Domain
{
    class BuildArtifact : IBuildArtifact
    {
        public BuildArtifact(
            IBuild build,
            string name,
            string fullName,
            long? size,
            DateTimeOffset modificationDateTime)
        {
           Build = build;
           Name = name;
           FullName = fullName;
           Size = size;
           ModificationDateTime = modificationDateTime;
        }

        public string Name { get; }
        public string FullName { get; }
        public long? Size { get; }
        public DateTimeOffset ModificationDateTime { get; }
        public IBuild Build { get; }

        public async Task Download(FileInfo outputFile)
            => await Build.DownloadArtifact(FullName, outputFile).ConfigureAwait(false);
        public async Task Download(Stream output)
            => await Build.DownloadArtifact(FullName, output).ConfigureAwait(false);
        public async Task<Stream> OpenArtifactInputStream()
            => await Build.OpenArtifactStream(FullName).ConfigureAwait(false);
    }
}