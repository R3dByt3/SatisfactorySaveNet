using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class BoxD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.BoxD;

    public Vector3D Min { get; set; }
    public Vector3D Max { get; set; }
    public bool IsValid { get; set; }
}