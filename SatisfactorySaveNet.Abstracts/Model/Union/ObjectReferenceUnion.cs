namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class ObjectReferenceUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.ObjectReference;
    public ObjectReference Value { get; set; } = new();

    public float Unknown1 { get; set; }
    public float Unknown2 { get; set; }
    public float Unknown3 { get; set; }
    public float Unknown4 { get; set; }
    public string Unknown5 { get; set; } = string.Empty;
}
