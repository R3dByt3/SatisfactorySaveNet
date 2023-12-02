using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectSerializer
{
    ComponentObject Deserialize(BinaryReader reader, Header header, ComponentObject componentObject);
}