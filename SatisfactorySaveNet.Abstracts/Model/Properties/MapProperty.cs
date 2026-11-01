using SatisfactorySaveNet.Abstracts.Model.Union;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class MapProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Map;

    public string KeyType { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public int ModeType { get; set; }
    public string? ModeUnk1 { get; set; }
    public string? ModeUnk2 { get; set; }
    public string? ModeUnk3 { get; set; }
    public required IDictionary<UnionBase, UnionBase?> Elements { get; set; }
}
