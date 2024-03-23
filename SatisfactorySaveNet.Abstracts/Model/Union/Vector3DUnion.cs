using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class Vector3DUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Vector3D;
    public Vector3D Value { get; set; }
}
