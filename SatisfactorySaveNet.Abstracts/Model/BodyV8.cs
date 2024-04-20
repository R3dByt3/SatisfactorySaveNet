using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class BodyV8 : BodyBase
{
    public override BodyConstraint BodyType => BodyConstraint.V8;

    /// <summary>
    /// Levels and the persistent level. There is one more level than the level count above, the last entry being the persistent level (See SCIM). For the format of one level
    /// </summary>
    public ICollection<Level> Levels { get; set; } = [];

    /// <summary>
    /// Unknown grid related data
    /// </summary>
    public Grid? Grid { get; set; }

    /// <summary>
    /// A list of object references, can also be ignored. for the format of one such ObjectReference
    /// </summary>
    [Obsolete("These information seem to be obsolete")]
    public ICollection<ObjectReference>? ObjectReferences { get; set; }
}