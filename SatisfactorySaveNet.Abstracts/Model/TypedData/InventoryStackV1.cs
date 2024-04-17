using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class InventoryStackV1 : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.InventoryStackV1;

    public required string Unknown1 { get; set; } = string.Empty;
    public required string Unknown2 { get; set; } = string.Empty;
    public required int Unknown3 { get; set; }
    public required int Unknown4 { get; set; }
    public required Property? Unknown5 { get; set; }
    public required string Unknown6 { get; set; } = string.Empty;
}
