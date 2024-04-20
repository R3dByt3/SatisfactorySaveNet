using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayIntProperty : IArrayProperty
{
    public ICollection<int> Values { get; set; } = [];
}
