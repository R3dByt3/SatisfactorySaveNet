using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayEnumProperty : IArrayProperty
{
    public ICollection<string> Values { get; set; } = [];
}
