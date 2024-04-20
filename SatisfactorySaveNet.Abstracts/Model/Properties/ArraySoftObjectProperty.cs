using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArraySoftObjectProperty : ArrayPropertyBase
{
    public override ArrayPropertyConstraint ArrayValueType => ArrayPropertyConstraint.SoftObject;

    public ICollection<SoftObjectReference> Values { get; set; } = [];
}
