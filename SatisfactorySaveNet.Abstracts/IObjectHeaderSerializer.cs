using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectHeaderSerializer
{
    public ComponentObject Deserialize(BinaryReader reader);
}