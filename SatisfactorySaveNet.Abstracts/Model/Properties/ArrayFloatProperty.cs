using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayFloatProperty : IArrayProperty
{
    public ICollection<float> Values { get; set; } = [];
}
