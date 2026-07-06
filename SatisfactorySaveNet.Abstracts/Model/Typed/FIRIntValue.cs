namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FIRIntValue : FIRAnyValue
{
    public override FIRTypeConstraint FIRType => FIRTypeConstraint.Int;

    public required int Value { get; set; }
}
