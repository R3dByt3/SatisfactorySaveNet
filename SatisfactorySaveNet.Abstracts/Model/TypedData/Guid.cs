namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Guid : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Guid;

    public string Value { get; set; } = string.Empty;
}
