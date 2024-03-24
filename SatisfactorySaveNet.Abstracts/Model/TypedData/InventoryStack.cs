using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class InventoryStack : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.InventoryStack;

    public IList<Property> Properties { get; set; } = [];
}
