namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class RotatorD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.RotatorD;

    public Maths.Vector.Vector3D Value { get; set; }
}