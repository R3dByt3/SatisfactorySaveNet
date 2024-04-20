namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class Int64Property : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Int64;

    public long Value { get; set; }
}