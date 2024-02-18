using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectReferenceSerializer
{
    public ObjectReference Deserialize(BinaryReader reader);
}