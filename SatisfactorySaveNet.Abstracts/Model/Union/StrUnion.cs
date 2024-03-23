namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class StrUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Str;
    public string Value { get; set; } = string.Empty;

    public float Unknown1 { get; set; }
    public float Unknown2 { get; set; }
    public float Unknown3 { get; set; }
}
