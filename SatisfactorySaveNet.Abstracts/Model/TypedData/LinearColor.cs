using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class LinearColor : ITypedData
{
    /// <summary>
    /// R, G, B, A
    /// </summary>
    public Vector4 Color { get; set; }
}