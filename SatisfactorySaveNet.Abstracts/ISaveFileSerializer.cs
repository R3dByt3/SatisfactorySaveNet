using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface ISaveFileSerializer
{
    public SatisfactorySave Deserialize(byte[] data);
    public SatisfactorySave Deserialize(Stream stream);
    public SatisfactorySave Deserialize(string path);
}