namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class Rotator : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Rotator;

    public Maths.Vector.Vector3 Value { get; set; }
}