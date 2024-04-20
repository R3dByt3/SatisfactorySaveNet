namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class Guid : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Guid;

    public string Value { get; set; } = string.Empty;
}
