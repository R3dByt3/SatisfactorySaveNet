using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class ExtraObject
{
    public int Unknown1 { get; set; }
    public required ICollection<ExtraInstance> Instances { get; set; }
    public required string ClassName { get; set; }
}
