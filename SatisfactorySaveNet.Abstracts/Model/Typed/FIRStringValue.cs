namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FIRStringValue : FIRAnyValue
{
    public override FIRTypeConstraint FIRType => FIRTypeConstraint.String;
    
    public required string Value { get; set; }
}