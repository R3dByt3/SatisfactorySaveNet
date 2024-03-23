namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class EnumUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Enum;
    public string Value { get; set; } = string.Empty;
}
