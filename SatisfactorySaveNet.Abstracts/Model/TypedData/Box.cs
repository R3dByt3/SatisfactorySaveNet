using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Box : ITypedData
{
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }
    public bool IsValid { get; set; }
}