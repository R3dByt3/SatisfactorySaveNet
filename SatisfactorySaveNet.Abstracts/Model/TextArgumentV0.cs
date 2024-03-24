namespace SatisfactorySaveNet.Abstracts.Model;

public class TextArgumentV0 : TextArgument
{
    public override byte ValueType => 0;
    public int ArgumentValue { get; set; }
    public int ArgumentValueUnknown { get; set; }

    public TextArgumentV0(string name, int argumentValue, int argumentValueUnknown) : base(name)
    {
        ArgumentValue = argumentValue;
        ArgumentValueUnknown = argumentValueUnknown;
    }
}
