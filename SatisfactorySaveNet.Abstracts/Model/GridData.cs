using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class GridData
{
    public string Unknown1 { get; set; } = string.Empty;
    public uint GridHex { get; set; }
    public uint Count { get; set; }
    public ICollection<GridLevel> Levels { get; set; } = [];
}
