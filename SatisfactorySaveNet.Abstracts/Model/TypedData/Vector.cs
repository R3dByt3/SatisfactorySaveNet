using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Vector : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Vector;

    public Vector3 Value { get; set; }
}