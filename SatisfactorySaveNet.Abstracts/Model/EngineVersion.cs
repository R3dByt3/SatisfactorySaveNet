namespace SatisfactorySaveNet.Abstracts.Model;

public class EngineVersion
{
    public ushort InMajor { get; set; }
    public ushort InMinor { get; set; }
    public ushort InPatch { get; set; }
    public uint InChangelist { get; set; }
    public string InBranch { get; set; } = string.Empty;
}
