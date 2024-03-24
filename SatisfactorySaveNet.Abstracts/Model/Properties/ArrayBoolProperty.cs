using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayBoolProperty : IArrayProperty
{
    /// <summary>
    /// Values[x] != 0 <=> True
    /// </summary>
    public IList<sbyte> Values { get; set; } = [];
}
