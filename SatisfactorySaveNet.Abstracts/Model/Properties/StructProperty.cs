using SatisfactorySaveNet.Abstracts.Model.TypedData;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class StructProperty : Property
{
    public string Type { get; set; } = string.Empty;
    public TypedData.TypedData Value { get; set; }

    public StructProperty(TypedData.TypedData value)
    {
        Value = value;
    }
}