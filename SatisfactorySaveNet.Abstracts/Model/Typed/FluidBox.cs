namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FluidBox : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FluidBox;

    public float Value { get; set; }
}