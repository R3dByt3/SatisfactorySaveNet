using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model;

public class TextArgumentV4 : TextArgument
{
    public override byte ValueType => 4;
    public TextProperty ArgumentPropertyValue { get; set; }

    public TextArgumentV4(string name, TextProperty argumentPropertyValue) : base(name)
    {
        ArgumentPropertyValue = argumentPropertyValue;
    }
}
