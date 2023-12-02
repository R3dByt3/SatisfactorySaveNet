using System.Numerics;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Quat : ITypedData
{
    public Quaternion Value { get; set; }
}