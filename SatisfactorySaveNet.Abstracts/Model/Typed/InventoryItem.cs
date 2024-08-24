using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class InventoryItem : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.InventoryItem;

    public string ItemType { get; set; } = string.Empty;
    public required ObjectReference? ObjectReference { get; set; }
    public Property? ExtraProperty { get; set; }
}