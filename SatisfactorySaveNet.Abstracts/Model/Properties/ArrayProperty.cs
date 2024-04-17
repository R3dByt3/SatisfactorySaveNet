namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayProperty : Property
{
    public string Type { get; set; } = string.Empty;
    public required IArrayProperty Property { get; set; }
}
