using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectReferenceSerializer
{
    public ObjectReference Deserialize(BinaryReader reader);
}