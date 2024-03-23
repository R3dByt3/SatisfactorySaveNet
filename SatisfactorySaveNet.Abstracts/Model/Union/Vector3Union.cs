using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class Vector3Union : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Vector3;
    public Vector3 Value { get; set; }
}
