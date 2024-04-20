namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class RailroadTrackPosition : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.RailroadTrackPosition;

    public string LevelName { get; set; } = string.Empty; //ToDo: ObjectReference
    public string PathName { get; set; } = string.Empty;
    public float Offset { get; set; }
    public float Forward { get; set; }
}