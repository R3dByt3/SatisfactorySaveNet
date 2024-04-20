using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayTextProperty : IArrayProperty
{
    public ICollection<TextProperty> Values { get; set; } = [];
}
