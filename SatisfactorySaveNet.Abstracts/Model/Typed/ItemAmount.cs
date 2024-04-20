namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class ItemAmount : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.ItemAmount;

    public int Unknown1 { get; set; }
    public string Unknown2 { get; set; } = string.Empty;
    public int Unknown3 { get; set; }
}
