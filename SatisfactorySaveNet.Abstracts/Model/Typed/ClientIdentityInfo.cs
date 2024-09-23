namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class ClientIdentityInfo : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.ClientIdentityInfo;

    public string Value { get; set; } = string.Empty;
}