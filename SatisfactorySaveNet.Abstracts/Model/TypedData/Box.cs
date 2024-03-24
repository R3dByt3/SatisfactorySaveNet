using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Box : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Box;

    public Maths.Vector.Vector3 Min { get; set; }
    public Maths.Vector.Vector3 Max { get; set; }
    public bool IsValid { get; set; }
}