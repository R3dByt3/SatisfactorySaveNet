using SatisfactorySaveNet.Abstracts.Model;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class LocomotiveData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.LocomotiveData;

    public int Count { get; set; }
    public required ICollection<CargoObject> CargoObjects { get; set; }
    public required ObjectReference Previous { get; set; }
    public required ObjectReference Next { get; set; }
}
