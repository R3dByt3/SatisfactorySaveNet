namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class ItemAmount : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.ItemAmount;

    public required ObjectReference Unknown1 { get; set; }
    public int Unknown2 { get; set; }
}
