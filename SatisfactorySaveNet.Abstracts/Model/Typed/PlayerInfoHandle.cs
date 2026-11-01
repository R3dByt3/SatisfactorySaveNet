namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class PlayerInfoHandle : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.PlayerInfoHandle;

    public byte ServiceProvider { get; set; }
    public int? PlayerInfoTableIndex { get; set; }
    public byte? LegacyPlayerInfoTableIndex { get; set; }
}
