using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayObjectProperty : IArrayProperty
{
    public IList<ObjectReference> Values { get; set; } = [];
}
