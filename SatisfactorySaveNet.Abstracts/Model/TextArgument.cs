namespace SatisfactorySaveNet.Abstracts.Model;

public abstract class TextArgument
{
    public required string Name { get; set; } = string.Empty;
    public abstract byte ValueType { get; }
}
