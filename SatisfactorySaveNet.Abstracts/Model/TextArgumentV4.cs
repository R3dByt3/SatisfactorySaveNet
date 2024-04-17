using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model;

public class TextArgumentV4 : TextArgument
{
    public override byte ValueType => 4;
    public required TextProperty ArgumentPropertyValue { get; set; }
}
