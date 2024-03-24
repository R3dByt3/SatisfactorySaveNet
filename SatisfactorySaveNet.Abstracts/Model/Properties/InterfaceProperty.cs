namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class InterfaceProperty : Property
{
    public ObjectReference Value { get; set; }

    public InterfaceProperty(ObjectReference value)
    {
        Value = value;
    }
}