using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Item
{
    public required ObjectReference Name { get; set; }
    public ObjectReference? ItemState { get; set; }
    /// <summary>
    /// Supposed to be a float, but Vec4 seems to be logical?
    /// </summary>
    public required Vector4I Position { get; set; }
}