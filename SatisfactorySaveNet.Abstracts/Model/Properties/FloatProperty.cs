namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class FloatProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Float;

    public float Value { get; set; }
}