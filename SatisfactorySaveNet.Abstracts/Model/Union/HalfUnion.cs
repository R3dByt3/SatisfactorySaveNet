using SatisfactorySaveNet.Abstracts.Maths.Data;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class HalfUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Half;
    public Half Value { get; set; }
}
