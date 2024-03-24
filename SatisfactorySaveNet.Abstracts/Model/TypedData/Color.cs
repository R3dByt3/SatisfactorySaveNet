using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Color : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Color;

    /// <summary>
    /// R, G, B, A (In sbytes!)
    /// </summary>
    public Vector4I Value { get; set; }
}