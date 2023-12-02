using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface ISaveFileSerializer
{
    public SatisfactorySave Deserialize(byte[] data);
    public SatisfactorySave Deserialize(Stream stream);
    public SatisfactorySave Deserialize(string path);
}