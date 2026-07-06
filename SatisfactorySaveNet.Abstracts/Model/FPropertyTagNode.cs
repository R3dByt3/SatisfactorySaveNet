using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class FPropertyTagNode
{
    public string Name { get; set; } = string.Empty;
    public ICollection<FPropertyTagNode> Children { get; set; } = [];
}
