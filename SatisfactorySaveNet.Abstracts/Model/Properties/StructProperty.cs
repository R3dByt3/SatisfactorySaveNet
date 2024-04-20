namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class StructProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Struct;

    public string Type { get; set; } = string.Empty;
    public required Typed.TypedData Value { get; set; }
}