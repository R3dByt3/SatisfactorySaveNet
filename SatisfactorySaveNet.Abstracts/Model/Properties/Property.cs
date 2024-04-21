namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public abstract class Property
{
    public abstract PropertyConstraint PropertyValueType { get; }

    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
}