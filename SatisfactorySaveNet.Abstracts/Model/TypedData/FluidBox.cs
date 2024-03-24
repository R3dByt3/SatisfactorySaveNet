namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class FluidBox : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FluidBox;

    public float Value { get; set; }
}