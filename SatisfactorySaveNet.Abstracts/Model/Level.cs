namespace SatisfactorySaveNet.Abstracts.Model;

public class Level
{
    public string Name { get; set; } = string.Empty;
    public IList<ObjectReference> Collectables { get; set; } = Array.Empty<ObjectReference>();
    public IList<ComponentObject> Objects { get; set; } = Array.Empty<ComponentObject>();
#pragma warning disable S1133
    [Obsolete("These information seem to be obsolete")]
#pragma warning restore S1133
    public IList<ObjectReference>? SecondCollectables { get; set; } = Array.Empty<ObjectReference>();
}