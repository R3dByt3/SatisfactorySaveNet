namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class Int64Union : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Int64;
    public long Value { get; set; }
}
