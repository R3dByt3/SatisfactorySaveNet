using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.ExtraData;

public class BlueprintData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.BlueprintData;

    public List<ObjectReference> Objects { get; set; }

    public BlueprintData(List<ObjectReference> objects)
    {
        Objects = objects;
    }
}