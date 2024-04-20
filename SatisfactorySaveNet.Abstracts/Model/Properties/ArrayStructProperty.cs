using SatisfactorySaveNet.Abstracts.Model.Typed;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayStructProperty : ArrayPropertyBase
{
    public override ArrayPropertyConstraint ArrayValueType => ArrayPropertyConstraint.Struct;

    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public (int, int, int, int) UUID { get; set; } = new(0, 0, 0, 0);
    public string ElementType { get; set; } = string.Empty;
    public ICollection<TypedData> Values { get; set; } = [];
}
