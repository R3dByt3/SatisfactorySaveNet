using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class LightweightBuildableSubsystem : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.LightweightBuildableSubsystem;

    public int Unknown1 { get; set; }
    public required ICollection<ExtraObject> Objects { get; set; }
    public int? LightWeightVersion { get; set; }
}
