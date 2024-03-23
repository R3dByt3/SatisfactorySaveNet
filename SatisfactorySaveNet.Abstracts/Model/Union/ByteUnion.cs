namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class ByteUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Byte;
    public sbyte Value { get; set; }
}