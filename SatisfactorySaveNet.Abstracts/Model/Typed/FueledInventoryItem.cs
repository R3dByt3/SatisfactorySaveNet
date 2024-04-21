using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FueledInventoryItem : InventoryItem
{
    public string ScriptName { get; set; } = string.Empty;
    public int Unknown1 { get; set; }
    public required ICollection<Property> Properties { get; set; }
}
