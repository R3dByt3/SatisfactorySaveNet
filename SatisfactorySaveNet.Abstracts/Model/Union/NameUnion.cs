namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class NameUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Name;
    public string Value { get; set; } = string.Empty;
}
