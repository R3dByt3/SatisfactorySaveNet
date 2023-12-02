namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayProperty : Property
{
    public string Type { get; set; } = string.Empty;
    public IArrayProperty Property { get; set; }

    public ArrayProperty(IArrayProperty property)
    {
        Property = property;
    }
}
