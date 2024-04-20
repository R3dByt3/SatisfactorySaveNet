namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class IntProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Int32;

    public int Value { get; set; }
}