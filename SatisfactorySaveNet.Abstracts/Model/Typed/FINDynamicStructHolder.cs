using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public abstract class FINDynamicStructHolder : TypedData
{
    public int Unknown1 { get; set; }
    public required string TypeName { get; set; }
    public required ICollection<Property> Properties { get; set; }
}
