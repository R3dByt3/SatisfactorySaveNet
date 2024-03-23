using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class Vector3IUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Vector3I;
    public Vector3I Value { get; set; }
}
