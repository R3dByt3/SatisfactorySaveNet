namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ObjectProperty : Property
{
    public ObjectReference Value { get; set; }
    public ObjectProperty(ObjectReference value)
    {
        Value = value;
    }
}