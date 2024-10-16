using SatisfactorySaveNet.Abstracts.Maths.Vector;
using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class PowerLineData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.PowerLineData;

    public int Count { get; set; }
    public required ObjectReference Source { get; set; }
    public required ObjectReference Target { get; set; }
    public Vector3? SourceTranslation { get; set; }
    public Vector3? TargetTranslation { get; set;}
}
