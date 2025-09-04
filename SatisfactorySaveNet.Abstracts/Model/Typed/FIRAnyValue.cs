namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public abstract class FIRAnyValue : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FIRAnyValue;
    public abstract FIRTypeConstraint FIRType { get; }

    public required sbyte ValueType { get; set; }
}
