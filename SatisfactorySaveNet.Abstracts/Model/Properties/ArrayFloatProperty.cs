using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayFloatProperty : IArrayProperty
{
    public IList<float> Values { get; set; } = [];
}
