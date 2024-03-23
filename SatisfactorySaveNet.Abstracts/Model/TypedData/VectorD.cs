using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class VectorD : ITypedData
{
    public Vector3D Value { get; set; }
}