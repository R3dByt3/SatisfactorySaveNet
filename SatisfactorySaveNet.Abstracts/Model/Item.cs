using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Item
{
    public string Name { get; set; } = string.Empty;
    public required ObjectReference ObjectReference { get; set; }
    /// <summary>
    /// Supposed to be a float, but Vec4 seems to be logical?
    /// </summary>
    public Vector4I Position { get; set; }
    public int Length { get; set; }
}