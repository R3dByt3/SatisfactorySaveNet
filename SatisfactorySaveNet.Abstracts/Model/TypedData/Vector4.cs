namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Vector4 : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Vector4;

    public Maths.Vector.Vector4 Value { get; set; }
}
