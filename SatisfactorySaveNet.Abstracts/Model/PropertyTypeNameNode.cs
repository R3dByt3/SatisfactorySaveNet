using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class PropertyTypeNameNode
{
    public string Name { get; set; } = string.Empty;
    public ICollection<PropertyTypeNameNode>? Nodes { get; set; }
}
