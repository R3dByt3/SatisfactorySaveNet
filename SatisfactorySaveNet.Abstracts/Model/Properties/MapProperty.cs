using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class MapProperty : Property
{
    public string KeyType { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public int ModeType { get; set; }
    public IDictionary<Property, Property> Elements { get; set; }

    public MapProperty(IDictionary<Property, Property> elements)
    {
        Elements = elements;
    }
}
