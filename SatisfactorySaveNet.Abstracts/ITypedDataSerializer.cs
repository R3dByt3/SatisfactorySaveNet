using SatisfactorySaveNet.Abstracts.Model.TypedData;

namespace SatisfactorySaveNet.Abstracts;

public interface ITypedDataSerializer
{
    public ITypedData Deserialize(BinaryReader reader, string type, long endPosition);
}
