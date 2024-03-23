namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class FloatUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Float;
    public float Value { get; set; }
}
