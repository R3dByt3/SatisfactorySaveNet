namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class StorageSingleUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.StorageSingle;
    public float Unknown1 { get; set; }
    public float Unknown2 { get; set; }
    public float Unknown3 { get; set; }
}
