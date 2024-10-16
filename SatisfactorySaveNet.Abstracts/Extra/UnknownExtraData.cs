namespace SatisfactorySaveNet.Abstracts.Extra;

public class UnknownExtraData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.UnknownExtraData;

    public string Missing { get; set; } = string.Empty;
}
