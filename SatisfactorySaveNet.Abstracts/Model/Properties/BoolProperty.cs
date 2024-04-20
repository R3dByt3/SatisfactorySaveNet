namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class BoolProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Bool;

    /// <summary>
    /// Value != 0 <=> True
    /// </summary>
    public sbyte Value { get; set; }
}