namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class StructProperty : Property
{
    public string Type { get; set; } = string.Empty;
    public required TypedData.TypedData Value { get; set; }
}