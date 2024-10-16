using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayUInt64Property : ArrayPropertyBase
{
    public override ArrayPropertyConstraint ArrayValueType => ArrayPropertyConstraint.UInt64;

    public ICollection<ulong> Values { get; set; } = [];
}
