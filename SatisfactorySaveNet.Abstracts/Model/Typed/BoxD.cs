using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class BoxD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.BoxD;

    public Vector3D Min { get; set; }
    public Vector3D Max { get; set; }
    /// <summary>
    /// IsValid != 0 <=> True
    /// </summary>
    public sbyte IsValid { get; set; }
}