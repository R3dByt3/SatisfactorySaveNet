namespace SatisfactorySaveNet.Abstracts.Model.Extra;

public class PlayerData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.PlayerData;

    public string? Missing { get; set; }
    public byte PlayerType { get; set; }
    public string? EpicOnlineServicesId { get; set; }
    public string? SteamId { get; set; }
    public string? PlatformId { get; set; }
}