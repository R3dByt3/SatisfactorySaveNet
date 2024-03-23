using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class TextUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Text;
    public TextProperty Value { get; set; } = new();
}
