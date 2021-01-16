namespace TeamCityRestClientNet.RestApi
{
    public class TriggerBuildRequestDto
    {
        public string BranchName { get; set; }
        public bool? Personal { get; set; }
        public TriggeringOptionsDto TriggeringOptions { get; set; }
        public ParametersDto Properties { get; set; }
        public BuildTypeDto BuildType { get; set; }
        public CommentDto Comment { get; set; }
        //  TODO: lastChanges
        //    <lastChanges>
        //      <change id="modificationId"/>
        //    </lastChanges>
    }

    public class TriggeringOptionsDto
    {
        public bool? CleanSources { get; set; }
        public bool? RebuildAllDependencies { get; set; }
        public bool? QueueAtTop { get; set; }
    }

    public class CommentDto
    {
        public string Text { get; set; }
    }

    public class TriggeredBuildDto
    {
        public int? Id { get; set; }
        public string BuildTypeId { get; set; }
    }

    public class BuildCancelRequestDto
    {
        public string Comment { get; set; } = "";
        public bool ReaddIntoQueue { get; set; } = false;
    }
}