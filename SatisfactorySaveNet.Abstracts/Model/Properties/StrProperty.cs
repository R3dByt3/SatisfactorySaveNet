namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class StrProperty : Property
{
    public string Value { get; set; } = string.Empty;
    public int Flags { get; set; }
    public sbyte HistoryType { get; set; }
    public bool IsCultureInvariant { get; set; }
}