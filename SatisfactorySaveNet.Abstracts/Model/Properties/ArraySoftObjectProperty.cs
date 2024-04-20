using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArraySoftObjectProperty : IArrayProperty
{
    public ICollection<SoftObjectReference> Values { get; set; } = [];
}
