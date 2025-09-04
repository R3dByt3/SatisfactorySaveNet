namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FIRFINDynamicStructHolderValue : FIRAnyValue
{
    public override FIRTypeConstraint FIRType => FIRTypeConstraint.FINDynamicStructHolder;
    
    public required FINDynamicStructHolder Value { get; set; }
}