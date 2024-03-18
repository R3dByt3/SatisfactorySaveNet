using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Body
{
    /// <summary>
    /// Levels and the persistent level. There is one more level than the level count above, the last entry being the persistent level (See SCIM). For the format of one level
    /// </summary>
    public IList<Level> Levels { get; set; } = [];

    /// <summary>
    /// A list of object references, can also be ignored. for the format of one such ObjectReference
    /// </summary>
    [Obsolete("These information seem to be obsolete")]
    public IList<ObjectReference>? ObjectReferences { get; set; }
}