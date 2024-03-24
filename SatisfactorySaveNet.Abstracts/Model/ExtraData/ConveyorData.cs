using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.ExtraData;

public class ConveyorData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.Conveyor;

    public List<Item> Items { get; set; }

    public ConveyorData(List<Item> items)
    {
        Items = items;
    }
}