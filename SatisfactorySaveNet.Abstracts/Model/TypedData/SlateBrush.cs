namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class SlateBrush : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.SlateBrush;

    public string Unknown { get; set; } = string.Empty;
}
