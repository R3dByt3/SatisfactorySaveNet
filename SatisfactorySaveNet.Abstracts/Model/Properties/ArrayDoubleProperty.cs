using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayDoubleProperty : IArrayProperty
{
    public IList<double> Values { get; set; } = [];
}
