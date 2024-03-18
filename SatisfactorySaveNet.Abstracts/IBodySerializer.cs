using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IBodySerializer
{
    public Body Deserialize(BinaryReader reader, Header header);
}