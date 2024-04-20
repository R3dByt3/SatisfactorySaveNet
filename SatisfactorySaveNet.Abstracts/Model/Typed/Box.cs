using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class Box : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Box;

    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }
    /// <summary>
    /// IsValid != 0 <=> True
    /// </summary>
    public sbyte IsValid { get; set; }
}