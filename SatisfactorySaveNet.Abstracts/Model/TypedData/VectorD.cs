using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class VectorD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.VectorD;

    public Vector3D Value { get; set; }
}