namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class RailroadTrackPosition : ITypedData
{
    public string LevelName { get; set; } = string.Empty;
    public string PathName { get; set; } = string.Empty;
    public float Offset { get; set; }
    public float Forward { get; set; }
}