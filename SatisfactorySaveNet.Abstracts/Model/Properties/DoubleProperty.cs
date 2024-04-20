namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class DoubleProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Double;

    public double Value { get; set; }
}