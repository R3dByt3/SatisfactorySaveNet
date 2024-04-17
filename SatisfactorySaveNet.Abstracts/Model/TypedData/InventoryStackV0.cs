namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class InventoryStackV0 : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.InventoryStackV0;

    public required int Unknown1 { get; set; }
    public required string Unknown2 { get; set; } = string.Empty;
    public required int Unknown3 { get; set; }
    public required int Unknown4 { get; set; }
    public required int Unknown5 { get; set; }
}
