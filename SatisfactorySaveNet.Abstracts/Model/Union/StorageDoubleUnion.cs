namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class StorageDoubleUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.StorageDouble;
    public double Unknown1 { get; set; }
    public double Unknown2 { get; set; }
    public double Unknown3 { get; set; }
}
