using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class PropertyData : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.PropertyData;

    public Property? Value { get; set; }
}
