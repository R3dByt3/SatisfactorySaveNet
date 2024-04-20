namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Array;

    public string Type { get; set; } = string.Empty;
    public required ArrayPropertyBase Property { get; set; }
}
