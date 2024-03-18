using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayBoolProperty : IArrayProperty
{
    public IList<bool> Values { get; set; } = [];
}
