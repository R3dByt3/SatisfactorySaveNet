namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class UInt32Property : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.UInt32;

    public uint Value { get; set; }
}
