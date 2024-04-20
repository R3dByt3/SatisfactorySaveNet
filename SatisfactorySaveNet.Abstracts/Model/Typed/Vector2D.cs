namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class Vector2D : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Vector2D;

    public Maths.Vector.Vector2D Value { get; set; }
}
