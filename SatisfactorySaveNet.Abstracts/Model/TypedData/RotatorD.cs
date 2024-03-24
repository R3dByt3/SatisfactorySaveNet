using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class RotatorD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.RotatorD;

    public Maths.Vector.Vector3D Value { get; set; }
}