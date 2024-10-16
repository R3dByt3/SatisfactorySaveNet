namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class IntVector4 : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.IntVector4;

    public Maths.Vector.Vector4I Value { get; set; }
}
