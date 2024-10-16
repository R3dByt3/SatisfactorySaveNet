using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class CircuitData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.CircuitData;

    public int Count { get; set; }
    public required ICollection<Circuit> Circuits { get; set; }
}
