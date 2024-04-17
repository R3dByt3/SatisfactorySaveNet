using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArraySoftObjectProperty : IArrayProperty
{
    public IList<SoftObjectReference> Values { get; set; } = [];
}
