using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class DroneStationAction
{
    public string Name { get; set; } = string.Empty;
    public required ICollection<Property> Properties { get; set; }
}
