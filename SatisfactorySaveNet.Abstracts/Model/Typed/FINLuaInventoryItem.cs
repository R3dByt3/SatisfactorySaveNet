namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FINLuaInventoryItem : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINLuaInventoryItem;

    public required ObjectReference Unknown1 { get; set; }
    public required int Unknown2 { get; set; }
    public required ObjectReference Unknown3 { get; set; }
}
