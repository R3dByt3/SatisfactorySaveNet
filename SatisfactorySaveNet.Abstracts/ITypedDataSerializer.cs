using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.TypedData;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface ITypedDataSerializer
{
    public ITypedData Deserialize(BinaryReader reader, Header header, string type, long endPosition);
}
