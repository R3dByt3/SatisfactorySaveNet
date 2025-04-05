namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class InventoryStack : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.InventoryStack;

    public required ObjectReference Unknown1 { get; set; }
    public required int Unknown2 { get; set; }
    public required int Unknown3 { get; set; }
    public required int Unknown4 { get; set; }
}
