using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class ArrayProperties : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.ArrayProperties;

    public IList<Property> Values { get; set; } = [];
    public string TypeName { get; set; } = string.Empty;
}
