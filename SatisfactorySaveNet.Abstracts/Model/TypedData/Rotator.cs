using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Rotator : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Rotator;

    public Maths.Vector.Vector3 Value { get; set; }
}