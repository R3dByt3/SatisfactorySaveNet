using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class IntPoint : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.IntPoint;

    public Vector2I Value { get; set; }
}
