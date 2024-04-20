using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayDoubleProperty : IArrayProperty
{
    public ICollection<double> Values { get; set; } = [];
}
