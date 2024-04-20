using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Typed;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface ITypedDataSerializer
{
    public TypedData Deserialize(BinaryReader reader, Header header, string type, bool isArrayProperty);
}
