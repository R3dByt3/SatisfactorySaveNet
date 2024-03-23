namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class DoubleUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Double;
    public double Value { get; set; }
}
