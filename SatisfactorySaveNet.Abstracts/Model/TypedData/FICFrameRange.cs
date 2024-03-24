namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class FICFrameRange : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FICFrameRange;

    public long Begin { get; set; }
    public long End { get; set; }
}
