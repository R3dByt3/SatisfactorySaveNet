namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class BoolUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Bool;
    public bool Value { get; set; }
}