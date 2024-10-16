using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class StatefulItem : Item
{
    internal int State { get; set; }
    public required ICollection<Property> Properties { get; set; }
}