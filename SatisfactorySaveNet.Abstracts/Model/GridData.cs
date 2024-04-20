using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class GridData
{
    public string Unknown1 { get; set; } = string.Empty;
    public int Unknown2 { get; set; }
    public int Unknown3 { get; set; }
    public ICollection<GridLevel> Levels { get; set; } = [];
}
