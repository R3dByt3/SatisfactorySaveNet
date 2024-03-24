using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayTextProperty : IArrayProperty
{
    public IList<TextProperty> Values { get; set; } = [];
}
