namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class TextProperty : Property
{
    public int Flags { get; set; }
    public sbyte HistoryType { get; set; }
    public bool IsCultureInvariant { get; set; }
    public string Value { get; set; } = string.Empty;
}