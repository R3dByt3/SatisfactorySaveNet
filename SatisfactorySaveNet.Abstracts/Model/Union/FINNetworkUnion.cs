using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class FINNetworkUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.FINNetwork;
    public required FINNetworkProperty Value { get; set; }
}
