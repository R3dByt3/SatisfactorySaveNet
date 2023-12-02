using SatisfactorySaveNet.Abstracts.Model.TypedData;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class StructProperty : Property
{
    public string Type { get; set; } = string.Empty;
    public ITypedData Value { get; set; }

    public StructProperty(ITypedData value)
    {
        Value = value;
    }
}