namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class SoftObjectProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.SoftObject;

    public required ObjectReference Value { get; set; }
}