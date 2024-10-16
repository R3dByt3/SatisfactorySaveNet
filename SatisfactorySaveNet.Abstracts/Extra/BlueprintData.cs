using SatisfactorySaveNet.Abstracts.Model;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class BlueprintData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.BlueprintData;

    public int Count { get; set; }
    public required ICollection<ObjectReference> Objects { get; set; }
}