using SatisfactorySaveNet.Abstracts.Model.Union;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class SetProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Set;

    public string Type { get; set; } = string.Empty;
    public int ModeType { get; set; }
    public string? ModeUnk1 { get; set; }
    public string? ModeUnk2 { get; set; }
    public string? ModeUnk3 { get; set; }
    public ICollection<UnionBase> Elements { get; set; } = [];
}