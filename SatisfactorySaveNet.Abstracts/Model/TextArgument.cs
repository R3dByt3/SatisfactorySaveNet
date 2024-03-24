using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model;

public abstract class TextArgument
{
    public string Name { get; set; } = string.Empty;
    public abstract byte ValueType { get; }

    public TextArgument(string name)
    {
        Name = name;
    }
}
