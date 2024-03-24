namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class UInt32Union : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.UInt32;
    public uint Value { get; set; }
}
