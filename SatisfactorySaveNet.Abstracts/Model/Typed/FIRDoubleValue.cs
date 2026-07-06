namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FIRDoubleValue : FIRAnyValue
{
    public override FIRTypeConstraint FIRType => FIRTypeConstraint.Double;

    public required double Value { get; set; }
}
