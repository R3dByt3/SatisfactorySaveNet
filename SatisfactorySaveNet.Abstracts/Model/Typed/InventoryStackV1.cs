using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class InventoryStackV1 : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.InventoryStackV1;

    public required ObjectReference Unknown1 { get; set; }
    public required int Unknown2 { get; set; }
    public required int Unknown3 { get; set; }
    public required Property? Unknown4 { get; set; }
    public required string Unknown5 { get; set; } = string.Empty;
}
