namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class IntUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Int;
    public int Value { get; set; }
}
