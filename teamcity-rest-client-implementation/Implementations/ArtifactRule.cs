using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Implementations
{
    class ArtifactRule : IArtifactRule
    {
        private readonly string _pathRule;

        public ArtifactRule(string pathRule)
        {
            this._pathRule = pathRule;
        }

        public bool Include => !this._pathRule.StartsWith("-:");

        public string SourcePath 
            => this._pathRule
                .SubstringBefore("=>")
                .SubstringBefore("!")
                .SubstringAfter(":");

        public string ArchivePath 
            => this._pathRule
                .SubstringBefore("=>")
                .SubstringAfter("!", "")
                .Let((str) => !string.IsNullOrEmpty(str) ? str : null);
        public string DestinationPath 
            => this._pathRule
                .SubstringAfter("=>", "")
                .Let((str) => !string.IsNullOrEmpty(str) ? str : null);
    }
}