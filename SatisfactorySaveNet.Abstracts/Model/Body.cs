namespace SatisfactorySaveNet.Abstracts.Model;

public class Body
{
    /// <summary>
    /// Levels and the persistent level. There is one more level than the level count above, the last entry being the persistent level (See SCIM). For the format of one level
    /// </summary>
    public IList<Level> Levels { get; set; } = Array.Empty<Level>();

    /// <summary>
    /// A list of object references, can also be ignored. for the format of one such ObjectReference
    /// </summary>
#pragma warning disable S1133
    [Obsolete("These information seem to be obsolete")]
#pragma warning restore S1133
    public IList<ObjectReference>? ObjectReferences { get; set; }
}