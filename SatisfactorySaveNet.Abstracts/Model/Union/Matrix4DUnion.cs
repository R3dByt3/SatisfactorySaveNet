using SatisfactorySaveNet.Abstracts.Maths.Matrix;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class Matrix4DUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Matrix4D;
    public Matrix4D Value { get; set; }
}
