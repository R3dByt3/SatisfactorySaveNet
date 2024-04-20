namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FINGPUT1BufferPixel : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINGPUT1BufferPixel;

    public string Character { get; set; } = string.Empty;
    public Maths.Vector.Vector4 ForegroundColor { get; set; }
    public Maths.Vector.Vector4 BackgroundColor { get; set; }
}
