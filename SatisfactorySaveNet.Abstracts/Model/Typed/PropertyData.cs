using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class PropertyData : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.PropertyData;

    public Property? Value { get; set; }
}
