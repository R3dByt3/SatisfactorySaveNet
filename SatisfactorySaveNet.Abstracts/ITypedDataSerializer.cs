using SatisfactorySaveNet.Abstracts.Model.TypedData;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface ITypedDataSerializer
{
    public ITypedData Deserialize(BinaryReader reader, string type, long endPosition);
}
