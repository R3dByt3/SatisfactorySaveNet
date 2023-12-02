namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayEnumProperty : IArrayProperty
{
    public IList<string> Values { get; set; } = Array.Empty<string>();
}
