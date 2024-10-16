using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class VehicleData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.VehicleData;

    public int Count { get; set; }
    public required ICollection<CargoObject> CargoObjects { get; set; }
}
