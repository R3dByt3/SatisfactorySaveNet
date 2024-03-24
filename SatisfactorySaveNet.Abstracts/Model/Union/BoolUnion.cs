namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class BoolUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Bool;
    /// <summary>
    /// Value != 0 <=> True
    /// </summary>
    public sbyte Value { get; set; }
}