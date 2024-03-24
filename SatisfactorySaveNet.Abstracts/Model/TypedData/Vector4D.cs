namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Vector4D : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Vector4D;

    public Maths.Vector.Vector4D Value { get; set; }
}
