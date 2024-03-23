namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class LBBalancerUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.LBBalancer;
    public int NormalIndex { get; set; }
    public int OverflowIndex { get; set; }
    public int FilterIndex { get; set; }
}
