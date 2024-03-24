using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Grid
{
    public string Unknown1 { get; set; } = string.Empty;
    public long Unknown2 { get; set; }
    public int Unknown3 { get; set; }
    public string Unknown4 { get; set; } = string.Empty;
    public int Unknown5 { get; set; }
    public IList<GridData> Data { get; set; } = [];
}
