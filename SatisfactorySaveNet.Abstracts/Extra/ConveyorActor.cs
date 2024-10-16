using SatisfactorySaveNet.Abstracts.Model;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class ConveyorActor
{
    public required ObjectReference Unknown1 { get; set; }
    public required ObjectReference ConveyorBase { get; set; }
    public required ICollection<Spline> Splines { get; set; }
    public float OffsetAtStart { get; set; }
    public float StartsAtLength { get; set; }
    public float EndsAtLength { get; set; }
    public int FirstItemIndex { get; set; }
    public int LastItemIndex { get; set; }
    public int IndexInChainArray { get; set; }
}
