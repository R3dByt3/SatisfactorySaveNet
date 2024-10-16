using SatisfactorySaveNet.Abstracts.Extra;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IExtraDataSerializer
{
    public ExtraData? Deserialize(BinaryReader reader, string typePath, Header header, long expectedPosition);
}
