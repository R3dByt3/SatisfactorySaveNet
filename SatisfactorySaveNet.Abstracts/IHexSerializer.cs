using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IHexSerializer
{
    public string Deserialize(BinaryReader reader, int length);
}
