using SatisfactorySaveNet.Abstracts.Model.Union;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class SetProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Set;

    public string Type { get; set; } = string.Empty;
    public ICollection<UnionBase> Elements { get; set; } = [];
}