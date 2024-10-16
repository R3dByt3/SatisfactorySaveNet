namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class UInt64Property : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.UInt64;

    public ulong Value { get; set; }
}
