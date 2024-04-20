using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayIntProperty : ArrayPropertyBase
{
    public override ArrayPropertyConstraint ArrayValueType => ArrayPropertyConstraint.Int32;

    public ICollection<int> Values { get; set; } = [];
}
