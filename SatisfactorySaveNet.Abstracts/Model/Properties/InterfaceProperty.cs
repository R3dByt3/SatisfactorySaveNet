namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class InterfaceProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Interface;

    public required ObjectReference Value { get; set; }
}