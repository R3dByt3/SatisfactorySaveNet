using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayInt64Property : ArrayPropertyBase
{
    public override ArrayPropertyConstraint ArrayValueType => ArrayPropertyConstraint.Int64;

    public ICollection<long> Values { get; set; } = [];
}
