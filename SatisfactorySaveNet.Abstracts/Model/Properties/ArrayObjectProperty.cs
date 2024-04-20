using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayObjectProperty : IArrayProperty
{
    public ICollection<ObjectReference> Values { get; set; } = [];
}
