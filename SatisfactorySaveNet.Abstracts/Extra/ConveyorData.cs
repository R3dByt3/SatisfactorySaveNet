using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Extra;

public class ConveyorData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.Conveyor;

    public int Count { get; set; }
    public required ICollection<Item> Items { get; set; }
}