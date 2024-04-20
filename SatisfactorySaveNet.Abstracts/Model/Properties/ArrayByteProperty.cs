using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayByteProperty : IArrayProperty
{
    public ICollection<sbyte> Values { get; set; } = [];
}
