namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class EnumProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Enum;

    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}