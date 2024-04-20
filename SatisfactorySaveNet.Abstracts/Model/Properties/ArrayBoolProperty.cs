using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayBoolProperty : ArrayPropertyBase
{
    public override ArrayPropertyConstraint ArrayValueType => ArrayPropertyConstraint.Bool;

    /// <summary>
    /// Values[x] != 0 <=> True
    /// </summary>
    public ICollection<sbyte> Values { get; set; } = [];
}
