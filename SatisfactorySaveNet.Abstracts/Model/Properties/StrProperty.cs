namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class StrProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.String;

    public string Value { get; set; } = string.Empty;
    public int Flags { get; set; }
    public sbyte HistoryType { get; set; }
    public bool IsCultureInvariant { get; set; }
}