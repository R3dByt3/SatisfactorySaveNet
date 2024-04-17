namespace SatisfactorySaveNet.Abstracts.Model;

public class TextArgumentV0 : TextArgument
{
    public override byte ValueType => 0;
    public required int ArgumentValue { get; set; }
    public required int ArgumentValueUnknown { get; set; }
}
