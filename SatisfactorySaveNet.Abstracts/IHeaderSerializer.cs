using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface IHeaderSerializer
{
    public Header Deserialize(BinaryReader reader);
}