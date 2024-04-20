using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FactoryCustomizationColorSlot : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FactoryCustomizationColorSlot;

    public ICollection<Property> Properties { get; set; } = [];
}
