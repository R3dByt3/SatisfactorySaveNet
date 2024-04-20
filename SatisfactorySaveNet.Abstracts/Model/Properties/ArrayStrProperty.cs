using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayStrProperty : IArrayProperty
{
    public ICollection<string> Values { get; set; } = [];
}
