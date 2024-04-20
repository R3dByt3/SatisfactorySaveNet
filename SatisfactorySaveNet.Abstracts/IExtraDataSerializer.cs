using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Extra;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IExtraDataSerializer
{
    public ExtraData? Deserialize(BinaryReader reader, string typePath, Header header, long expectedPosition);
}
