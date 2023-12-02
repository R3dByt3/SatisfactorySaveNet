using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts;

public interface IPropertySerializer
{
    public IEnumerable<Property> DeserializeProperties(BinaryReader reader);
    public Property? DeserializeProperty(BinaryReader reader, string? type = null);
}
