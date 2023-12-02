using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class InventoryItem : ITypedData
{
    public string ItemType { get; set; } = string.Empty;
    public string LevelName { get; set; } = string.Empty;
    public string PathName { get; set; } = string.Empty;
    public Property? ExtraProperty { get; set; }
}