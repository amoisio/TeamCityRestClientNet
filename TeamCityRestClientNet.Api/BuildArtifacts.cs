using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IBuildArtifact
    {
        /** Artifact name without path. e.g. my.jar */
        string Name { get; }
        /** Artifact name with path. e.g. directory/my.jar */
        string FullName { get; }
        long? Size { get; }
        DateTimeOffset ModificationDateTime { get; }
        IBuild Build { get; }
        Task Download(FileInfo outputFile);
        Task Download(Stream output);
        Task<Stream> OpenArtifactInputStream();
    }

    public interface IArtifactDependency
    {
        AsyncLazy<IBuildConfiguration> DependsOnBuildConfiguration { get; }
        string Branch { get; }
        List<IArtifactRule> ArtifactRules { get; }
        bool CleanDestinationDirectory { get; }
    }

    public interface IArtifactRule
    {
        bool Include { get; }
        /**
         * Specific file, directory, or wildcards to match multiple files can be used. Ant-like wildcards are supported.
         */
        string SourcePath { get; }
        /**
         * Follows general rules for sourcePath: ant-like wildcards are allowed.
         */
        string ArchivePath { get; }
        /**
         * Destination directory where files are to be placed.
         */
        string DestinationPath { get; }
    }
}