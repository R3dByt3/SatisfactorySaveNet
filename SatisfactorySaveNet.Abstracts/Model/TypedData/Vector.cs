using System.Numerics;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Vector : ITypedData
{
    public Vector3 Value { get; set; }
}