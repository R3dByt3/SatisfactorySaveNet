namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class TextProperty : Property
{
    public int Flags { get; set; }
    public byte HistoryType { get; set; }

    /// <summary>
    /// IsCultureInvariant != 0 <=> True
    /// </summary>
    public int? IsCultureInvariant { get; set; }
    public string? Value { get; set; }
    public string? TextKey { get; set; }
    public string? TableId { get; set; }
    public byte? TransformType { get; set; }
    public TextProperty? SourceText { get; set; }
    public TextProperty? SourceFmt { get; set; }
    public string? Key { get; set; }
    public string? NameSpace { get; set; }
    public TextArgument[]? Arguments { get; set; }
}