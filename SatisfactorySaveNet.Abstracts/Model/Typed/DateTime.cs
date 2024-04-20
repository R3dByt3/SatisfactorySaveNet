namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class DateTime : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.DateTime;

    public System.DateTime Value { get; set; }
}
