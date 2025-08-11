namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FIRAnyValue : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FIRAnyValue;

    public required sbyte ValueType { get; set; }
    public required string Value { get; set; }
}
