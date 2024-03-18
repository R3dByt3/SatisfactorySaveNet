using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayStrProperty : IArrayProperty
{
    public IList<string> Values { get; set; } = [];
}
