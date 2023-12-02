using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class InventoryStack : ITypedData
{
    public IList<Property> Properties { get; set; } = Array.Empty<Property>();
}
