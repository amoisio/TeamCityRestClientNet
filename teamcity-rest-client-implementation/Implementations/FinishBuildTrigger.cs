using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Implementations
{
    class FinishBuildTrigger : IFinishBuildTrigger
    {
        private readonly TriggerDto _dto;

        public FinishBuildTrigger(TriggerDto dto)
        {
            this._dto = dto;
        }

        public BuildConfigurationId InitiatedBuildConfiguration {
            get {
                var dependsOn = this._dto.Properties?.Property
                    ?.FirstOrDefault(prop => prop.Name == "dependsOn")
                    ?.Value ?? throw new NullReferenceException();

                return new BuildConfigurationId(dependsOn);
            }
        }
        public bool AfterSuccessfulBuildOnly {
            get {
                var afterBuild = this._dto.Properties?.Property
                    ?.FirstOrDefault(prop => prop.Name == "afterSuccessfulBuildOnly")
                    ?.Value;

                return Boolean.TryParse(afterBuild, out bool result)
                    ? result
                    : false;
            }
        }

        public HashSet<string> IncludedBranchPatterns 
            => this.BranchPatterns
                .Where(pattern => !pattern.StartsWith("-:"))
                .Select(pattern => pattern.SubstringAfter(":"))
                .ToHashSet();
        public HashSet<string> ExcludedBranchPatterns
            => this.BranchPatterns
                .Where(pattern => pattern.StartsWith("-:"))
                .Select(pattern => pattern.SubstringAfter(":"))
                .ToHashSet();

        private List<string> BranchPatterns
            => this._dto.Properties
                ?.Property
                ?.FirstOrDefault(prop => prop.Name == "branchFilter")
                ?.Value
                ?.Split(" ")
                ?.ToList()
                ?? new List<string>();
    }
}