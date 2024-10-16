using SatisfactorySaveNet.Abstracts.Model;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class ConveyorChainActor : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.ConveyorChainActor;

    public int Count { get; set; }
    public required ObjectReference Unknown1 { get; set; }
    public required ObjectReference Unknown2 { get; set; }
    public required ICollection<ConveyorActor> ConveyorActors { get; set; }
    public float TotalLength { get; set; }
    public int NumberItems { get; set; }
    public int HeadItemIndex { get; set; }
    public int TailItemIndex { get; set; }
    public required ICollection<Item> Items { get; set; }
}
