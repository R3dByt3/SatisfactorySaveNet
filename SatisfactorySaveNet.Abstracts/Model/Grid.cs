using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Grid
{
    public string Unknown1 { get; set; } = string.Empty;
    public long Unknown2 { get; set; }
    public uint HeadHex1 { get; set; }
    public string Unknown4 { get; set; } = string.Empty;
    public uint HeadHex2 { get; set; }
    public ICollection<GridData> Data { get; set; } = [];
}
