namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class Int8Property : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Int8;

    public sbyte Value { get; set; }
}
