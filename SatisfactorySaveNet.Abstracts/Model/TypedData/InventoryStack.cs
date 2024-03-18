using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class InventoryStack : ITypedData
{
    public IList<Property> Properties { get; set; } = [];
}
