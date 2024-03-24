namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class BoolProperty : Property
{
    /// <summary>
    /// Value != 0 <=> True
    /// </summary>
    public sbyte Value { get; set; }
}