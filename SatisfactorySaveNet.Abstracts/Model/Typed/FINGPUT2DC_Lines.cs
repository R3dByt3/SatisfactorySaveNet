using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FINGPUT2DC_Lines : FINDynamicStructHolder
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINGPUT2DC_Lines;
    public required string Unknown2 { get; set; }
    public required string Unknown3 { get; set; }
    public int Unknown4 { get; set; }
    public int Unknown5 { get; set; }
    public required string Unknown6 { get; set; }
    public byte Unknown7 { get; set; }
    public int Unknown8 { get; set; }
    public required Property Unknown9 { get; set; }
    public required ICollection<Maths.Vector.Vector2D> Unknown10 { get; set; }
}
