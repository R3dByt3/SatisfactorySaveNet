using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class Vector4Union : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Vector4;
    public Vector4 Value { get; set; }
}
