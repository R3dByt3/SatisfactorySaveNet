using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class FactoryCustomizationColorSlot : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FactoryCustomizationColorSlot;

    public IList<Property> Properties { get; set; } = [];
}
