using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.ExtraData;

public class ConveyorExtraData : IExtraData
{
    public List<Item> Items { get; set; }

    public ConveyorExtraData(List<Item> items)
    {
        Items = items;
    }
}