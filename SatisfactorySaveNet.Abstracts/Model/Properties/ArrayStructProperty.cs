using SatisfactorySaveNet.Abstracts.Model.TypedData;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayStructProperty : IArrayProperty
{
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public (int, int, int, int) UUID { get; set; } = new(0, 0, 0, 0);
    public string ElementType { get; set; } = string.Empty;
    public IList<ITypedData> Values { get; set; } = Array.Empty<ITypedData>();
}
