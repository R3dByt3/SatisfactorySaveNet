namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class NameProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Name;

    public string Value { get; set; } = string.Empty;
}