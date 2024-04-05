namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class FINNetworkProperty : Property
{
    public string LevelName { get; set; } = string.Empty; //ToDo: ObjectReference
    public string PathName { get; set; } = string.Empty;
    public FINNetworkProperty? Previous { get; set; }
    public string? Step { get; set; }
}
