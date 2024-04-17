using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface ISoftObjectReferenceSerializer
{
    public SoftObjectReference Deserialize(BinaryReader reader);
}