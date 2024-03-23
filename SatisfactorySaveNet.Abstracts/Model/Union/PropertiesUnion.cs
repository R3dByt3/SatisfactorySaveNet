using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class PropertiesUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Properties;
    public Property[] Value { get; set; } = [];
}
