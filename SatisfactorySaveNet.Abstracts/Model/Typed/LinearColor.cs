namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class LinearColor : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.LinearColor;

    /// <summary>
    /// R, G, B, A
    /// </summary>
    public Maths.Vector.Vector4 Color { get; set; }
}