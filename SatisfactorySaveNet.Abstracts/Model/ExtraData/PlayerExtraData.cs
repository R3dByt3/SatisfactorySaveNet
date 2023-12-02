namespace SatisfactorySaveNet.Abstracts.Model.ExtraData;

public class PlayerExtraData : IExtraData
{
    public string Missing { get; set; } = string.Empty;
    public sbyte PlayerType { get; set; }
    public string EpicOnlineServicesId { get; set; } = string.Empty;
    public string SteamId { get; set; } = string.Empty;
    public string PlatformId { get; set; } = string.Empty;
}