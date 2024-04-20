using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayInt64Property : IArrayProperty
{
    public ICollection<long> Values { get; set; } = [];
}
