using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

[Obsolete("This is a legacy body format, used by save games before the Update 8")]
public class BodyPreV8 : BodyBase
{
    public override BodyConstraint BodyType => BodyConstraint.PreV8;

    public ICollection<ObjectReference> Collectables { get; set; } = [];
    public ICollection<ComponentObject> Objects { get; set; } = [];
}
