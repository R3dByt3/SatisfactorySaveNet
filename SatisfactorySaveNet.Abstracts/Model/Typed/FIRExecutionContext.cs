namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FIRExecutionContext : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FIRExecutionContext;

    public required int Unknown1 { get; set; }
    public required FINNetworkTrace Trace { get; set; }
}
