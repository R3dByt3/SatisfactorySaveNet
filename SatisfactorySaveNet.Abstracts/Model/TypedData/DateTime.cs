namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class DateTime : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.DateTime;

    public System.DateTime Value { get; set; }
}
