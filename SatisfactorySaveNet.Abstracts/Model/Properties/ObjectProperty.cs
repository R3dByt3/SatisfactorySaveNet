namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ObjectProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Object;

    public required ObjectReference Value { get; set; }
}