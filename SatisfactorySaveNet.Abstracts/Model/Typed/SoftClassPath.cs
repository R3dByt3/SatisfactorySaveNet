namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class SoftClassPath : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.SoftClassPath;

    public string PackageName { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public string SubPathString { get; set; } = string.Empty;
}
