using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class TypedDataCollection : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Collection;
    public required ICollection<Property> Properties { get; set; }
}
