using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IBodySerializer
{
    public BodyBase Deserialize(BinaryReader reader, Header header);
}